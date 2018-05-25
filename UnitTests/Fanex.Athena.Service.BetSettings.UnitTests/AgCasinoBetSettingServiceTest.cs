namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using System.Linq;
    using BetSettings;
    using BetSettings.Interfaces;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using Model.BetSettings.DTO.AgCasino;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class AgCasinoBetSettingServiceTest
    {
        private const string FakeApiUrl = "http://api.com";

        private IAuthorizationService _authorizationService;
        private IAgCasinoBetSettingDataAccess _dataAccess;
        private IAppSettings _appSettings;
        private IWebClientService _webClientService;
        private IAgCasinoBetSettingService _service;
        private ICoreLogger _logger;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IAgCasinoBetSettingDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _logger = Substitute.For<ICoreLogger>();
            _service = new AgCasinoBetSettingService(
                _authorizationService,
                _logger,
                _dataAccess,
                _appSettings,
                _webClientService);

            const string ApiUrlName = "AGCasinoApiUrl";
            _appSettings.GetValue(ApiUrlName).Returns(FakeApiUrl);
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
            var licenseeId = FakeHelper.Create<int>();
            var licenseeName = FakeHelper.Create<string>();
            var custId = FakeHelper.Create<int>();
            var gameType = FakeHelper.Create<string>();
            var groupType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();

            var fakeSetting = new AgCasinoBetSetting
            {
                AgGroup = group,
                CheckedGroups = new[] { "B" }
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(custId).Returns(fakeSetting);
            _webClientService.UploadString(new Uri(FakeApiUrl), $"SiteName={licenseeName}&Currency=0")
                .Returns($"<Game GameType='{gameType}' Name='{group}'><Group GroupType='{groupType}'><Min>1</Min><Max>100</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(licenseeId, custId, licenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Games[0].GameType, Is.EqualTo(gameType));
                Assert.That(result.Games[0].Groups[0].GroupType, Is.EqualTo(groupType));
                Assert.That(result.BetSetting.CheckedGroups.Contains(group), Is.True);
            });
        }

        [Test]
        public void GetViewModel_CheckGroupsContainAgGroupInSetting_ReturnCorrectViewModel()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var licenseeName = FakeHelper.Create<string>();
            var custId = FakeHelper.Create<int>();
            var gameType = FakeHelper.Create<string>();
            var groupType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();

            var fakeSetting = new AgCasinoBetSetting
            {
                AgGroup = group,
                CheckedGroups = new[] { group, "B", "C" },
                IsEnabled = true
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(custId).Returns(fakeSetting);
            _webClientService.UploadString(new Uri(FakeApiUrl), $"SiteName={licenseeName}&Currency=0")
                .Returns($"<Game GameType='{gameType}' Name='{group}'><Group GroupType='{groupType}'><Min>1</Min><Max>100</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(licenseeId, custId, licenseeName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Games[0].GameType, Is.EqualTo(gameType));
                Assert.That(result.Games[0].Groups[0].GroupType, Is.EqualTo(groupType));
                Assert.That(result.BetSetting.CheckedGroups.Length, Is.EqualTo(3));
                Assert.That(result.IsEnabled, Is.True);
            });
        }

        [Test]
        public void GetViewModel_HaveNoPermission_ReturnNull()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var licenseeName = FakeHelper.Create<string>();
            var custId = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(false);

            // Action
            var result = _service.GetViewModel(licenseeId, custId, licenseeName);

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetViewModel_GetExceptionWhileProcessing_WriteErrorLog()
        {
            // Arrange
            var licenseeId = FakeHelper.Create<int>();
            var licenseeName = FakeHelper.Create<string>();
            var custId = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(licenseeId, custId, 0).Returns(true);

            _dataAccess.GetBetSetting(custId).Returns(FakeHelper.Create<AgCasinoBetSetting>());

            _webClientService.UploadString(new Uri(FakeApiUrl), $"SiteName={licenseeName}&Currency=0")
                .Returns("Wrong format");

            // Action
            var result = _service.GetViewModel(licenseeId, custId, licenseeName);

            // Assert
            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Test]
        public void UpdateAgCasinoBetSetting_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = FakeHelper.Create<ResultInfo>();
            var group = FakeHelper.Create<string>();
            var setting = new CommonBetSetting { LicenseeId = FakeHelper.Create<int>(), CustId = FakeHelper.Create<int>() };

            _authorizationService.HasPermissionOnData(setting.LicenseeId, setting.CustId, 0).Returns(true);
            _dataAccess.GetBetSetting(setting.CustId).Returns(new AgCasinoBetSetting
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
        public void UpdateAgCasinoBetSetting_SettingParamIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = "Invalid parameter. AG Casino is null"
            };

            // Action
            var result = _service.UpdateAgCasinoBetSetting(null, FakeHelper.Create<string>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void UpdateAgCasinoBetSetting_IsEnabledOfSettingIsFalse_ReturnCorrectErrorResult()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.AccessDeniedMsg
            };

            var setting = new CommonBetSetting { LicenseeId = FakeHelper.Create<int>(), CustId = FakeHelper.Create<int>() };

            _authorizationService.HasPermissionOnData(setting.LicenseeId, setting.CustId, 0).Returns(true);

            _dataAccess.GetBetSetting(setting.CustId).Returns(new AgCasinoBetSetting
            {
                IsEnabled = false
            });

            // Action
            var result = _service.UpdateAgCasinoBetSetting(setting, FakeHelper.Create<string>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void UpdateAgCasinoBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.AccessDeniedMsg
            };
            var group = FakeHelper.Create<string>();
            var setting = new CommonBetSetting { LicenseeId = FakeHelper.Create<int>(), CustId = FakeHelper.Create<int>() };

            _authorizationService.HasPermissionOnData(setting.LicenseeId, setting.CustId, 0).Returns(false);

            _dataAccess.GetBetSetting(setting.CustId).Returns(new AgCasinoBetSetting
            {
                IsEnabled = true
            });

            // Action
            var result = _service.UpdateAgCasinoBetSetting(setting, group);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }

        [Test]
        public void UpdateStatus_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = FakeHelper.Create<ResultInfo>();

            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var language = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.UpdateStatus(userId, custId, language, ip, true).Returns(expectedResult);

            // Action
            var result = _service.UpdateStatus(licenseeId, userId, custId, language, ip, true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            var expectedResult = new ResultInfo
            {
                Code = 1,
                Message = ViewResources.AccessDeniedMsg
            };
            var licenseeId = FakeHelper.Create<int>();
            var userId = FakeHelper.Create<int>();
            var custId = FakeHelper.Create<int>();
            var language = FakeHelper.Create<string>();
            var ip = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.UpdateStatus(licenseeId, userId, custId, language, ip, true);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(expectedResult.Code));
                Assert.That(result.Message, Is.EqualTo(expectedResult.Message));
            });
        }
    }
}