namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;
    using Utility.Interfaces;

    [TestFixture]
    public class GoldDeluxeBetSettingTest
    {
        private const int LicenseeId = 1;
        private const string SiteName = "spondemo";
        private const int CustId = 2;

        private IAuthorizationService _authorizationService;
        private IThirdPartyBetSettingDataAccess _dataAccess;
        private IAppSettings _appSettings;
        private IWebClientService _webClientService;
        private IGoldDeluxeBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IThirdPartyBetSettingDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _service = new GoldDeluxeBetSettingService(_authorizationService, _dataAccess, _appSettings, _webClientService);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(CustId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting()
            {
                IsLineEnabled = true,
                IsEnabled = true
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _appSettings = null;
            _webClientService = null;
            _service = null;
        }

        [Test]
        public void GetViewModel_WhenCall_ApiResultRetriviedCorrectly()
        {
            // Arrange
            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns("http://api1.com");
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns("http://api2.com");
            _webClientService.UploadString(new Uri("http://api1.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&Currency=0")
                .Returns("<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>WTF</ReturnMessage></GetUserGroupBettingLimit>");
            _webClientService.UploadString(new Uri("http://api2.com"), "SiteName=spondemo&Currency=0&ThirdPartyId=5")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='AA'><Min>1</Min><Max>100</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, SiteName);

            // Assert
            Assert.That(result.BetSetting.Group, Is.EqualTo("WTF"));
            Assert.That(result.Games[0].GameType, Is.EqualTo("1"));
            Assert.That(result.Games[0].Groups[0].GroupType, Is.EqualTo("AA"));
        }

        [Test]
        public void UpdateBetSetting_ApiReturnCodeIsZero_ReturnSuccessResult()
        {
            // Arrange
            var group = "A";
            _appSettings.GetValue("GoldDeluxeSetUserGroupApiUrl").Returns("http://api3.com");

            _webClientService.UploadString(new Uri("http://api3.com"), "SiteName=spondemo&IBCCustID=2&ThirdPartyId=5&GroupType=" + group)
                .Returns("<SetUserGroupBettingLimit><ReturnCode>0</ReturnCode></SetUserGroupBettingLimit>");

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, SiteName, CustId, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(0));
        }

        [Test]
        public void UpdateStatus_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 999
            };
            var userId = 1;
            var language = "en-US";
            var ip = "ip";
            _dataAccess.UpdateStatus(ProductTypes.GoldDeluxe, userId, CustId, language, ip, true).Returns(expectedResult);

            // Action
            var result = _service.UpdateStatus(LicenseeId, userId, CustId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }
    }
}