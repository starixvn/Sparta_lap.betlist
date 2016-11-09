namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.BetSettings.DTO.AgCasino;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;
    using Utility.Interfaces;

    [TestFixture]
    public class AgCasinoBetSettingServiceTest
    {
        private IAuthorizationService _authorizationService;
        private IAgCasinoBetSettingDataAccess _dataAccess;
        private IAppSettings _appSettings;
        private IWebClientService _webClientService;
        private IAgCasinoBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IAgCasinoBetSettingDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _service = new AgCasinoBetSettingService(_authorizationService, _dataAccess, _appSettings, _webClientService);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void GetViewModel_WhenCall_ApiResultRetriviedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var licenseeName = "spondemo";
            var custId = 2;
            _appSettings.GetValue("AGCasinoApiUrl").Returns("http://api.com");
            _dataAccess.GetBetSetting(custId).Returns(new AgCasinoBetSetting()
            {
                AgGroup = "A",
                CheckedGroups = new string[] { "B" }
            });
            _webClientService.UploadString(new Uri("http://api.com"), "SiteName=spondemo&Currency=0")
                .Returns("<Game GameType='1' Name='A'><Group GroupType='AA'><Min>1</Min><Max>100</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(licenseeId, custId, licenseeName);

            // Assert
            Assert.That(result.Games[0].GameType, Is.EqualTo("1"));
            Assert.That(result.Games[0].Groups[0].GroupType, Is.EqualTo("AA"));
        }

        [Test]
        public void UpdateAgCasinoBetSetting_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo()
            {
                Code = 999
            };
            var group = "A";
            var setting = new CommonBetSetting()
            {
                CustId = 2
            };
            _dataAccess.GetBetSetting(setting.CustId).Returns(new AgCasinoBetSetting()
            {
                IsEnabled = true
            });
            _dataAccess.UpdateBetSetting(Arg.Any<CommonBetSetting>(), group).Returns(expectedResult);

            // Action
            var result = _service.UpdateAgCasinoBetSetting(setting, group);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }

        [Test]
        public void UpdateStatus_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new ResultInfo()
            {
                Code = 999
            };
            var licenseeId = 1;
            var userId = 2;
            var custId = 3;
            var language = "en-US";
            var ip = "ip";
            _dataAccess.UpdateStatus(userId, custId, language, ip, true).Returns(expectedResult);

            // Action
            var result = _service.UpdateStatus(licenseeId, userId, custId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }
    }
}