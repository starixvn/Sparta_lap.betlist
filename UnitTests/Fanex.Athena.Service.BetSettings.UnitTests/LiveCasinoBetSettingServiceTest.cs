namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class LiveCasinoBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int UserId = 2;
        private const int CustId = 3;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private ILiveCasinoBetSettingDataAccess _dataAccess;
        private ILiveCasinoBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<ILiveCasinoBetSettingDataAccess>();
            _service = new LiveCasinoBetSettingService(_authorizationService, _dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void UpdateStatus_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var status = 4;
            var ip = "ip";
            var isSyncLiveCasino = true;

            // Action
            var result = _service.UpdateStatus(LicenseeId, UserId, CustId, status, Language, ip, isSyncLiveCasino);

            // Assert
            _dataAccess.Received().UpdateStatus(UserId, CustId, status, Language, ip);
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var status = 4;
            var ip = "ip";
            var isSyncLiveCasino = true;

            // Action
            var result = _service.UpdateStatus(LicenseeId, UserId, CustId, status, Language, ip, isSyncLiveCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateStatus_SyncLiveCasinoValueIsFalse_ReturnErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var status = 4;
            var ip = "ip";
            var isSyncLiveCasino = false;

            // Action
            var result = _service.UpdateStatus(LicenseeId, UserId, CustId, status, Language, ip, isSyncLiveCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Live Casino Database is not currently available. Please try again later."));
        }

        [Test]
        public void GetViewModel_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var gameTypeId = 1;
            var betSettings = new List<LiveCasinoBetSetting>
            {
                new LiveCasinoBetSetting
                {
                    GameTypeId = gameTypeId, GameType = "Game1"
                }
            };
            _dataAccess.GetBetSetting(CustId, Language).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, Language);

            // Assert
            Assert.That(result.LiveCasinoData[0].GameTypeId, Is.EqualTo(gameTypeId));
        }

        [Test]
        public void UpdateBetSetting_PassTwoBetSettingItems_DataAccessExecuteTwoTimes()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var ip = "ip";
            var isDisableLiveCasino = false;
            var betSettings = new List<LiveCasinoBetSetting>
            {
                new LiveCasinoBetSetting(),
                new LiveCasinoBetSetting()
            };
            _dataAccess.UpdateBetSetting(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<LiveCasinoBetSetting>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(new ResultInfo(), new ResultInfo());

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, UserId, CustId, betSettings, ip, Language, isDisableLiveCasino);

            // Assert
            _dataAccess.Received(2).UpdateBetSetting(UserId, CustId, Arg.Any<LiveCasinoBetSetting>(), ip, Language);
        }

        [Test]
        public void UpdateBetSetting_DisableLiveCasino_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var ip = "ip";
            var isDisableLiveCasino = true;
            var betSettings = new List<LiveCasinoBetSetting>();

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, UserId, CustId, betSettings, ip, Language, isDisableLiveCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var ip = "ip";
            var isDisableLiveCasino = false;
            var betSettings = new List<LiveCasinoBetSetting>();

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, UserId, CustId, betSettings, ip, Language, isDisableLiveCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateBetSetting_HaveOneErrorResultInBetSettingList_ReturnCorrectResultInfo()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var ip = "ip";
            var isDisableLiveCasino = false;
            var betSettings = new List<LiveCasinoBetSetting>
            {
                new LiveCasinoBetSetting(),
                new LiveCasinoBetSetting()
            };
            var resultInfo1 = new ResultInfo { Code = 0 };
            var resultInfo2 = new ResultInfo { Code = 1, Message = "Error" };

            _dataAccess.UpdateBetSetting(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<LiveCasinoBetSetting>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(resultInfo1, resultInfo2);

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, UserId, CustId, betSettings, ip, Language, isDisableLiveCasino);

            // Assert
            Assert.That(result.Code, Is.EqualTo(-1));
            Assert.That(result.Message, Is.EqualTo("Error, "));
        }
    }
}