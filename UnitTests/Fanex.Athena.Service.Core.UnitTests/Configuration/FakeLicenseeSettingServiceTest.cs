namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.UnitTest.Common.Helpers;
    using Model.Core.DTO.Configuration;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FakeLicenseeSettingServiceTest
    {
        private const string SettingJsonContent =
            @"[{ ""SiteName"": ""IvoryBet"", ""DisableBetList"": true, ""MinDate"": ""02/01/2018"", ""MaxDate"": ""yesterday"" }]";

        private FakeLicenseeSettingService _fakeLicenseeSettingService;
        private IMemoryCacheService _cache;
        private ISessionService _sessionService;
        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = "IvoryBet"
            });

            _fakeLicenseeSettingService = Substitute.ForPartsOf<FakeLicenseeSettingService>(_cache, _sessionService);
            _fakeLicenseeSettingService.When(x => x.GetSettingFileContent()).DoNotCallBase();
        }

        [Test]
        public void GetSetting_FileContentEmpty_ReturnNull()
        {
            // Arrange
            _fakeLicenseeSettingService.GetSettingFileContent().Returns(string.Empty);
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<FakeLicenseeSetting>)null);

            // Action
            var result = _fakeLicenseeSettingService.GetSetting();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetSetting_CacheNotContainData_SetCacheWithCorrectData()
        {
            // Arrange
            const string LicenseeName = "IvoryBet";

            _fakeLicenseeSettingService.GetSettingFileContent().Returns(SettingJsonContent);
            var fakeData = new[]
            {
                new FakeLicenseeSetting
                {
                    DisableBetList = true,
                    MaxDate = "yesterday",
                    MinDate = "02/01/2018",
                    SiteName = LicenseeName
                }
            };

            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<FakeLicenseeSetting>)null);

            // Action
            _fakeLicenseeSettingService.GetSetting();

            // Assert
            _cache.Received(1).Set(
                Arg.Any<string>(),
                Arg.Is<IEnumerable<FakeLicenseeSetting>>(x =>
                    ObjectHelper.ToJson(x) == ObjectHelper.ToJson(fakeData)));
        }

        [Test]
        public void GetSetting_CacheContainData_ReturnSettingFromCache()
        {
            // Arrange
            _fakeLicenseeSettingService.GetSettingFileContent().Returns(SettingJsonContent);
            var licenseeName = FakeHelper.Create<string>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = licenseeName
            });
            var fakeData = new[]
            {
                new FakeLicenseeSetting { SiteName = licenseeName }
            };
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);

            // Action
            var result = _fakeLicenseeSettingService.GetSetting();

            // Assert
            Assert.That(result.SiteName, Is.EqualTo(licenseeName));
        }
    }
}