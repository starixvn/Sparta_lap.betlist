namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.UnitTest.Common.Helpers;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeSettingServiceTest
    {
        private const string SettingJsonContent =
            @"[{ ""SiteName"": ""IvoryBet"", ""DisableBetList"": true, ""MinDate"": ""02/01/2018"", ""MaxDate"": ""yesterday"" }]";

        private LicenseeSettingService _licenseeSettingService;
        private IMemoryCacheService _cache;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();

            _licenseeSettingService = Substitute.ForPartsOf<LicenseeSettingService>(_cache);
            _licenseeSettingService.When(x => x.GetFakeLicenseeSettingContent()).DoNotCallBase();
        }

        [Test]
        public void GetSetting_FileContentEmpty_ReturnNull()
        {
            // Arrange
            _licenseeSettingService.GetFakeLicenseeSettingContent().Returns(string.Empty);
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<FakeLicenseeSetting>)null);

            // Action
            var result = _licenseeSettingService.GetFakeLicenseeSetting("xxx");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetSetting_CacheNotContainData_SetCacheWithCorrectData()
        {
            // Arrange
            const string LicenseeName = "IvoryBet";

            _licenseeSettingService.GetFakeLicenseeSettingContent().Returns(SettingJsonContent);
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
            _licenseeSettingService.GetFakeLicenseeSetting("xx");

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
            _licenseeSettingService.GetFakeLicenseeSettingContent().Returns(SettingJsonContent);
            var licenseeName = FakeHelper.Create<string>();
            var fakeData = new[]
            {
                new FakeLicenseeSetting { SiteName = licenseeName }
            };
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);

            // Action
            var result = _licenseeSettingService.GetFakeLicenseeSetting(licenseeName);

            // Assert
            Assert.That(result.SiteName, Is.EqualTo(licenseeName));
        }
    }
}