namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ReportSettingServiceTest
    {
        private ReportSettingService _reportSettingService;

        private IMemoryCacheService _cache;

        private IAppSettings _appSettings;

        private IFakeLicenseeSettingService _fakeLicenseeSettingService;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _appSettings = Substitute.For<IAppSettings>();
            _fakeLicenseeSettingService = Substitute.For<IFakeLicenseeSettingService>();
            _reportSettingService = Substitute.ForPartsOf<ReportSettingService>(_cache, _appSettings, _fakeLicenseeSettingService);
            _reportSettingService.When(x => x.GetSettingFileContent()).DoNotCallBase();
        }

        [Test]
        public void GetSetting_FileContentEmpty_ReturnNull()
        {
            // Arrange
            _reportSettingService.GetSettingFileContent().Returns(string.Empty);
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<ReportSetting>)null);

            // Action
            var result = _fakeLicenseeSettingService.GetSetting();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetSetting_CacheNotContainData_SetCacheWithCorrectData()
        {
            // Arrange
            const string ReportKey = "BalanceReport";
            _reportSettingService.GetSettingFileContent().Returns(@"[{ ""ReportKey"": ""BalanceReport"" }]");
            var fakeData = new[]
            {
                new ReportSetting { ReportKey = ReportKey }
            };

            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<ReportSetting>)null);

            // Action
            _reportSettingService.GetSetting(ReportKey);

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
            const string ReportKey = "Abc";
            _reportSettingService.GetSettingFileContent().Returns(string.Empty);

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = ReportKey }
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);

            // Action
            var result = _reportSettingService.GetSetting(ReportKey);

            // Assert
            Assert.That(result, Is.EqualTo(fakeData[0]));
        }

        [Test]
        public void GetSetting_FakeLicenseeSettingExists_AssignSettingFromFakeLicensee()
        {
            // Arrange
            const string LicenseeName = "spon";
            const string ReportKey = "Abc";

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = ReportKey }
            };
            var fakeLicenseeSetting = new FakeLicenseeSetting
            {
                SiteName = LicenseeName,
                MinDate = "2012/1/1",
                MaxDate = "2013/2/2",
                DisableBetList = true,
                LimitRecentMonths = 100
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);
            _fakeLicenseeSettingService.GetSetting().Returns(fakeLicenseeSetting);

            // Action
            var result = _reportSettingService.GetSetting(ReportKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.MinDateString, Is.EqualTo(fakeLicenseeSetting.MinDate));
                Assert.That(result.MaxDateString, Is.EqualTo(fakeLicenseeSetting.MaxDate));
                Assert.That(result.IsDisableBetList, Is.EqualTo(fakeLicenseeSetting.DisableBetList));
                Assert.That(result.LimitRecentMonths, Is.EqualTo(fakeLicenseeSetting.LimitRecentMonths));
            });
        }

        [TestCase(0, "2012/1/1")]
        [TestCase(1, "2018/4/1")]
        [TestCase(2, "2018/3/1")]
        [TestCase(3, "2018/2/1")]
        [TestCase(4, "2018/1/1")]
        public void GetSetting_FakeLicensee_GetCorrectMinDate(int limitRecentMonths, string expectedMinDate)
        {
            // Arrange
            const string ReportKey = "Abc";

            var fakeData = new[]
            {
                new ReportSetting { ReportKey = ReportKey }
            };
            var fakeLicenseeSetting = new FakeLicenseeSetting
            {
                SiteName = "123",
                MinDate = "2012/1/1",
                MaxDate = "2018/4/4",
                LimitRecentMonths = limitRecentMonths
            };
            _cache.Get<IEnumerable<ReportSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);
            _fakeLicenseeSettingService.GetSetting().Returns(fakeLicenseeSetting);

            // Action
            var result = _reportSettingService.GetSetting(ReportKey);

            // Assert
            Assert.That(result.MinDate.ToString("yyyy/M/d"), Is.EqualTo(expectedMinDate));
        }
    }
}