namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System;
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Configuration;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ReportSettingServiceTest
    {
        private ReportSettingService _reportSettingService;

        private IMemoryCacheService _cache;

        private IAppSettings _appSettings;

        private ILicenseeSettingService _fakeLicenseeSettingService;

        private ISessionStorage _sessionStorage;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _appSettings = Substitute.For<IAppSettings>();
            _fakeLicenseeSettingService = Substitute.For<ILicenseeSettingService>();
            _sessionStorage = Substitute.For<ISessionStorage>();
            _reportSettingService = Substitute.ForPartsOf<ReportSettingService>(
                _cache, 
                _appSettings, 
                _fakeLicenseeSettingService,
                _sessionStorage);
            _reportSettingService.When(x => x.GetSettingFileContent()).DoNotCallBase();
        }

        [Test]
        public void GetSetting_FileContentEmpty_ReturnNull()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();

            _reportSettingService.GetSettingFileContent().Returns(string.Empty);
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<ReportSetting>)null);

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetSetting_FileContentHasData_ReturnCorrectResult()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();

            var expectedResult = new List<ReportSetting> { new ReportSetting { ReportKey = reportKey } };
            var jsonContent = JsonConvert.SerializeObject(expectedResult);

            _reportSettingService.GetSettingFileContent().Returns(jsonContent);
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<ReportSetting>)null);
            _fakeLicenseeSettingService.GetFakeLicenseeSetting(Arg.Any<string>()).Returns(x => { return null; });

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.That(result.ReportKey, Is.EqualTo(reportKey));
        }

        [Test]
        public void GetSetting_CacheNotContainData_SetCacheWithCorrectData()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();
            _reportSettingService.GetSettingFileContent().Returns($@"[{{ ""ReportKey"": ""{reportKey}"" }}]");
            var fakeData = new[]
            {
                new ReportSetting { ReportKey = reportKey }
            };

            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<ReportSetting>)null);

            // Action
            _reportSettingService.GetSetting(reportKey);

            // Assert
            _cache.Received(1).Set(
                Arg.Any<string>(),
                Arg.Is<IEnumerable<ReportSetting>>(x =>
                    ObjectHelper.ToJson(x) == ObjectHelper.ToJson(fakeData)));
        }

        [Test]
        public void GetSetting_CacheContainData_ReturnSettingFromCache()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();
            _reportSettingService.GetSettingFileContent().Returns(string.Empty);

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = reportKey }
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.That(result, Is.EqualTo(fakeData[0]));
        }

        [Test]
        public void GetSetting_FakeLicenseeSettingExists_AssignSettingFromFakeLicensee()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = reportKey }
            };
            var fakeLicenseeSetting = new FakeLicenseeSetting
            {
                MinDate = "2012/1/1",
                MaxDate = "2013/2/2",
                DisableBetList = true,
                LimitRecentMonths = 0
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);
            _fakeLicenseeSettingService.GetFakeLicenseeSetting(Arg.Any<string>()).Returns(fakeLicenseeSetting);

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MinDateString, Is.EqualTo(fakeLicenseeSetting.MinDate));
                Assert.That(result.MaxDateString, Is.EqualTo(fakeLicenseeSetting.MaxDate));
                Assert.That(result.IsDisableBetList, Is.EqualTo(fakeLicenseeSetting.DisableBetList));
                Assert.That(result.LimitRecentMonths, Is.EqualTo(fakeLicenseeSetting.LimitRecentMonths));
            });
        }

        [Test]
        public void GetSetting_FakeLicenseeSettingExists_MinDateMaxDateIsYesterdayAndToday_AssignCorrectDate()
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = reportKey }
            };
            var fakeLicenseeSetting = new FakeLicenseeSetting
            {
                MinDate = "yesterday",
                MaxDate = "today",
                DisableBetList = true,
                LimitRecentMonths = 0
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);
            _fakeLicenseeSettingService.GetFakeLicenseeSetting(Arg.Any<string>()).Returns(fakeLicenseeSetting);

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MinDate, Is.EqualTo(DateTime.Today.AddDays(-1)));
                Assert.That(result.MaxDate, Is.EqualTo(DateTime.Today));
            });
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GetSetting_FakeLicenseeHasLimitMinDate_MinDateIsLessThanLimit_ReAssignMinDate(byte limitRecentMonths)
        {
            // Arrange
            var reportKey = FakeHelper.Create<string>();
            var fakeData = new[]
            {
                new ReportSetting { ReportKey = reportKey }
            };
            var fakeLicenseeSetting = new FakeLicenseeSetting
            {
                SiteName = "123",
                MinDate = DateTime.Today.AddMonths(-limitRecentMonths - 1).ToString("yyyy/M/d"),
                LimitRecentMonths = limitRecentMonths
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);
            _fakeLicenseeSettingService.GetFakeLicenseeSetting(Arg.Any<string>()).Returns(fakeLicenseeSetting);

            var expectedMinDate = DateTime.Today.AddMonths(-limitRecentMonths + 1).ToString("yyyy/M/1");

            // Action
            var result = _reportSettingService.GetSetting(reportKey);

            // Assert
            Assert.That(result.MinDate.ToString("yyyy/M/d"), Is.EqualTo(expectedMinDate));
        }
    }
}