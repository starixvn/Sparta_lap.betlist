namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using BetSettings;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
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

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void UpdateStatus_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            var status = 4;
            var ip = "ip";
            var isSyncLiveCasino = true;

            // Action
            var result = _service.UpdateStatus(LicenseeId, UserId, CustId, status, Language, ip, isSyncLiveCasino);

            // Assert
            _dataAccess.Received().UpdateStatus(UserId, CustId, status, Language, ip);
        }

        [Test]
        public void GetViewModel_WhenCall_DataAccessExecuteWithCorrectParams()
        {
            // Arrange
            var gameTypeId = 1;
            var betSettings = new List<LiveCasinoBetSetting>()
            {
                new LiveCasinoBetSetting()
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
            var ip = "ip";
            var isDisableLiveCasino = false;
            var betSettings = new List<LiveCasinoBetSetting>()
            {
                new LiveCasinoBetSetting(),
                new LiveCasinoBetSetting()
            };

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, UserId, CustId, betSettings, ip, Language, isDisableLiveCasino);

            // Assert
            _dataAccess.Received(2).UpdateBetSetting(UserId, CustId, Arg.Any<LiveCasinoBetSetting>(), ip, Language);
        }
    }
}