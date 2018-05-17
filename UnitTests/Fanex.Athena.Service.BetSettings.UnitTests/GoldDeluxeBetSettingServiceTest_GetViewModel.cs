namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System;
    using System.Linq;
    using BetSettings;
    using BetSettings.Interfaces;
    using Constant.DataDomain;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Extension.Utility;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class GoldDeluxeBetSettingTest
    {
        private const int GoldDeluxeThirdPartyId = 5;
        private const string DefaultGroup = "default";
        private const string UserGroupApiUrl = "http://api1.com";
        private const string BettingLimitApiUrl = "http://api2.com";
        private const string SetUserGroupApiUrl = "http://api3.com";

        private readonly int _licenseeId = FakeHelper.Create<int>();
        private readonly string _siteName = FakeHelper.Create<string>();
        private readonly int _custId = FakeHelper.Create<int>();
        private readonly int _currencyId = FakeHelper.Create<int>();

        private IAuthorizationService _authorizationService;
        private IThirdPartyBetSettingDataAccess _dataAccess;
        private IAppSettings _appSettings;
        private IWebClientService _webClientService;
        private IGoldDeluxeBetSettingService _service;
        private ICoreLogger _logger;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IThirdPartyBetSettingDataAccess>();
            _appSettings = Substitute.For<IAppSettings>();
            _webClientService = Substitute.For<IWebClientService>();
            _logger = Substitute.For<ICoreLogger>();
            _service = new GoldDeluxeBetSettingService(
                _authorizationService,
                _logger,
                _dataAccess,
                _appSettings,
                _webClientService);

            _appSettings.GetValue("GoldDeluxeGetUserGroupApiUrl").Returns(UserGroupApiUrl);
            _appSettings.GetValue("GoldDeluxeGroupBettingLimitApiUrl").Returns(BettingLimitApiUrl);
            _appSettings.GetValue("GoldDeluxeSetUserGroupApiUrl").Returns(SetUserGroupApiUrl);
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
        public void GetViewModel_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(false);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeUserGroup_ReturnCodeIsNotZero_ReturnEmpty()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi("<GetUserGroupBettingLimit><ReturnCode>1</ReturnCode><ReturnMessage>Message</ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi(string.Empty);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.BetSetting.Group, Is.Empty);
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeUserGroup_ReturnCodeIsZero_ReturnApiReturnedMessage()
        {
            // Arrange
            var expectedMessage = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi($"<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>{expectedMessage}</ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi(string.Empty);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.BetSetting.Group, Is.EqualTo(expectedMessage));
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeUserGroup_GetErrorWhileProcessing_WriteErrorLog()
        {
            // Arrange
            var expectedMessage = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi("Wrong format");
            SetUpGetBettingLimitApi(string.Empty);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeGroupBettingLimit_ApiReturnEmptyData_ReturnEmptyGameList()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi("<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage></ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi(string.Empty);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.Games, Is.Empty);
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeGroupBettingLimit_ApiReturnData_ReturnGamesFromApi()
        {
            // Arrange
            var gameType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();
            var groupType = FakeHelper.Create<string>();
            var min = FakeHelper.Create<int>();
            var max = FakeHelper.Create<int>();

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi("<GetUserGroupBettingLimit><ReturnCode>1</ReturnCode><ReturnMessage></ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi($"<Game GameType='{gameType}' Name='{group}'><Group GroupType='{groupType}'><Min>{min}</Min><Max>{max}</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);
            var firstGame = result.Games.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstGame.GameType, Is.EqualTo(gameType));
                Assert.That(firstGame.Name, Is.EqualTo(group));
                Assert.That(firstGame.Groups.First().GroupType, Is.EqualTo(groupType));
                Assert.That(firstGame.Groups.First().Min, Is.EqualTo(min));
                Assert.That(firstGame.Groups.First().Max, Is.EqualTo(max));
            });
        }

        [Test]
        public void GetViewModel_GetGoldDeluxeGroupBettingLimit_GetErrorWhileProcessing_WriteErrorLog()
        {
            // Arrange
            var expectedMessage = FakeHelper.Create<string>();

            _authorizationService.HasPermissionOnData(_licenseeId, _custId, 0).Returns(true);

            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = true,
                CurrencyId = _currencyId
            });

            SetUpGetUserGroupApi(string.Empty);
            SetUpGetBettingLimitApi("Wrong format");

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            _logger.Received().Error(Arg.Any<Exception>());
        }

        [Test]
        public void GetViewModel_IsNotLineEnabledValue_ReturnCorrectViewModelWithoutCheckedGroups()
        {
            // Arrange
            var setting = new ThirdPartyBetSetting
            {
                IsLineEnabled = false,
                IsEnabled = FakeHelper.Create<bool>(),
                CurrencyId = _currencyId
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(setting);

            SetUpGetUserGroupApi("<GetUserGroupBettingLimit><ReturnCode>1</ReturnCode><ReturnMessage></ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi("<Game GameType='1' Name='2'><Group GroupType='3'><Min>4</Min><Max>5</Max></Group></Game>");

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BetSetting.IsEnabled, Is.EqualTo(setting.IsEnabled));
                Assert.That(result.BetSetting.IsLineEnabled, Is.EqualTo(setting.IsLineEnabled));
                Assert.That(result.BetSetting.CurrencyId, Is.EqualTo(setting.CurrencyId));
                Assert.That(result.BetSetting.CheckedGroups, Is.Null);
            });
        }

        [Test]
        public void GetViewModel_IsLineEnabledValue_GamesIsEmpty_ReturnCorrectViewModelWithoutCheckedGroups()
        {
            // Arrange
            var setting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = FakeHelper.Create<bool>(),
                CurrencyId = _currencyId
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(setting);

            SetUpGetUserGroupApi("<GetUserGroupBettingLimit><ReturnCode>1</ReturnCode><ReturnMessage></ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi(string.Empty);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BetSetting.IsEnabled, Is.EqualTo(setting.IsEnabled));
                Assert.That(result.BetSetting.IsLineEnabled, Is.EqualTo(setting.IsLineEnabled));
                Assert.That(result.BetSetting.CurrencyId, Is.EqualTo(setting.CurrencyId));
                Assert.That(result.BetSetting.CheckedGroups, Is.Null);
            });
        }

        [Test]
        public void GetViewModel_IsLineEnabledAndHaveGames_GetCheckedGroupsFromGameList()
        {
            // Arrange
            var gameType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();
            var groupType1 = FakeHelper.Create<string>();
            var groupType2 = FakeHelper.Create<string>();
            var min = FakeHelper.Create<int>();
            var max = FakeHelper.Create<int>();

            var setting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = FakeHelper.Create<bool>(),
                CurrencyId = _currencyId
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(setting);

            SetUpGetUserGroupApi($"<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>{groupType1}</ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi($"<Game GameType='{gameType}' Name='{group}'>"
                + $"<Group GroupType='{groupType1}'><Min>1</Min><Max>2</Max></Group>"
                + $"<Group GroupType='{groupType2}'><Min>1</Min><Max>2</Max></Group>"
                + "</Game>");

            var expectedCheckedGroups = new string[] { groupType1, groupType2 };

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.BetSetting.CheckedGroups, Is.EqualTo(expectedCheckedGroups));
        }

        [Test]
        public void GetViewModel_IsLineEnabledAndHaveGames_SettingGroupIsEmpty_UpdateToDefault()
        {
            // Arrange
            var gameType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();
            var groupType1 = FakeHelper.Create<string>();
            var groupType2 = FakeHelper.Create<string>();
            var min = FakeHelper.Create<int>();
            var max = FakeHelper.Create<int>();

            var setting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = FakeHelper.Create<bool>(),
                CurrencyId = _currencyId
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(setting);

            SetUpGetUserGroupApi($"<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage></ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi($"<Game GameType='{gameType}' Name='{group}'>"
                + $"<Group GroupType='{groupType1}'><Min>1</Min><Max>2</Max></Group>"
                + $"<Group GroupType='{groupType2}'><Min>1</Min><Max>2</Max></Group>"
                + $"<Group GroupType='{DefaultGroup}'><Min>1</Min><Max>2</Max></Group>"
                + "</Game>");

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.BetSetting.Group, Is.EqualTo(DefaultGroup));
        }

        [Test]
        public void GetViewModel_IsLineEnabledAndHaveGames_SettingGroupIsNotInCheckedGroups_AddSettingGroupToCheckedGroups()
        {
            // Arrange
            var gameType = FakeHelper.Create<string>();
            var group = FakeHelper.Create<string>();
            var settingGroup = FakeHelper.Create<string>();
            var groupType1 = FakeHelper.Create<string>();
            var groupType2 = FakeHelper.Create<string>();
            var min = FakeHelper.Create<int>();
            var max = FakeHelper.Create<int>();

            var setting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true,
                IsEnabled = FakeHelper.Create<bool>(),
                CurrencyId = _currencyId
            };

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            _dataAccess.GetBetSetting(_custId, ProductTypes.GoldDeluxe).Returns(setting);

            SetUpGetUserGroupApi($"<GetUserGroupBettingLimit><ReturnCode>0</ReturnCode><ReturnMessage>{settingGroup}</ReturnMessage></GetUserGroupBettingLimit>");
            SetUpGetBettingLimitApi($"<Game GameType='{gameType}' Name='{group}'>"
                + $"<Group GroupType='{groupType1}'><Min>1</Min><Max>2</Max></Group>"
                + $"<Group GroupType='{groupType2}'><Min>1</Min><Max>2</Max></Group>"
                + "</Game>");

            var expectedCheckedGroups = (new string[] { groupType1, groupType2, settingGroup }).OrderBy(x => x);

            // Action
            var result = _service.GetViewModel(_licenseeId, _custId, _siteName);

            // Assert
            Assert.That(result.BetSetting.CheckedGroups, Is.EqualTo(expectedCheckedGroups));
        }

        private void SetUpGetBettingLimitApi(string returnValue)
        {
            _webClientService.UploadString(new Uri(BettingLimitApiUrl), $"SiteName={_siteName}&Currency={_currencyId}&ThirdPartyId={GoldDeluxeThirdPartyId}")
                .Returns(returnValue);
        }

        private void SetUpGetUserGroupApi(string returnValue)
        {
            _webClientService.UploadString(
                new Uri(UserGroupApiUrl),
                $"SiteName={_siteName}&IBCCustID={_custId}&ThirdPartyId={GoldDeluxeThirdPartyId}&Currency={_currencyId}")
            .Returns(returnValue);
        }
    }
}