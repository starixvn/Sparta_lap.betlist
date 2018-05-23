namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Users.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.UnitTest.Common.Helpers;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Interfaces;
    using Session.Model;

    [TestFixture]
    public class LicenseeSettingServiceTest
    {
        private const string SettingJsonContent =
            @"[{ ""SiteName"": ""IvoryBet"", ""DisableBetList"": true, ""MinDate"": ""02/01/2018"", ""MaxDate"": ""yesterday"" }]";

        private LicenseeSettingService _licenseeSettingService;
        private IMemoryCacheService _cache;
        private ISessionStorage _sessionStorage;

        [SetUp]
        public void SetUp()
        {
            _cache = Substitute.For<IMemoryCacheService>();
            _sessionStorage = Substitute.For<ISessionStorage>();
            _sessionStorage.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = "IvoryBet"
            });

            _licenseeSettingService = Substitute.ForPartsOf<LicenseeSettingService>(_cache, _sessionStorage);
            _licenseeSettingService.When(x => x.GetFakeLicenseeSettingContent()).DoNotCallBase();
        }

        [Test]
        public void GetSetting_FileContentEmpty_ReturnNull()
        {
            // Arrange
            _licenseeSettingService.GetFakeLicenseeSettingContent().Returns(string.Empty);
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs((IEnumerable<FakeLicenseeSetting>)null);

            // Action
            var result = _licenseeSettingService.GetFakeLicenseeSetting();

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
            _licenseeSettingService.GetFakeLicenseeSetting();

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
            _sessionStorage.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = licenseeName
            });
            var fakeData = new[]
            {
                new FakeLicenseeSetting { SiteName = licenseeName }
            };
            _cache.Get<IEnumerable<FakeLicenseeSetting>>(string.Empty).ReturnsForAnyArgs(fakeData);

            // Action
            var result = _licenseeSettingService.GetFakeLicenseeSetting();

            // Assert
            Assert.That(result.SiteName, Is.EqualTo(licenseeName));
        }

        [Test]
        public void GetSetting_LicenseeIsNull_ReturnNull()
        {
            // Arrange
            _sessionStorage.LicenseeSession.Returns(x => { return null; });

            // Action
            var result = _licenseeSettingService.GetFakeLicenseeSetting();

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void IsFakeLicensee_SettingIsNull_ReturnTrue()
        {
            // Arrange
            _sessionStorage.LicenseeSession.Returns(x => { return null; });

            // Action
            var result = _licenseeSettingService.IsFakeLicensee();

            // Assert
            Assert.That(result, Is.True);
        }
    }
}