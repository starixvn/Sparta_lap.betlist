namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections;
    using System.Collections.Generic;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class NumberGameBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int UserId = 2;
        private const int CustId = 3;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private INumberGameBetSettingDataAccess _dataAccess;
        private INumberGameBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<INumberGameBetSettingDataAccess>();
            _service = new NumberGameBetSettingService(_authorizationService, _dataAccess);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            var betSettings = new List<BetSetting>
            {
                new BetSetting()
            };
            _dataAccess.GetBetSetting(CustId).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            var data = new Hashtable
            {
                { "minbet161", 111m },
                { "maxbet161", 222m },
                { "maxpermatch161", 333m },
                { "maxperball", 444m }
            };
            var setting = new CommonBetSetting
            {
                CustId = CustId,
                UserId = UserId,
                Language = Language,
                Ip = "ip"
            };

            // Action
            var result = _service.UpdateBetSetting(setting, data);

            // Assert
            _dataAccess.Received().UpdateBetSetting(Arg.Is<NumberGameBetSetting>(
                x => x.CustId == CustId &&
                    x.UserId == UserId &&
                    x.Language == Language &&
                    x.Ip == "ip" &&
                    x.MinBet == 111m &&
                    x.MaxBet == 222m &&
                    x.MaxPerMatch == 333m &&
                    x.MaxPerBall == 444m &&
                    x.IsDataChange));
        }
    }
}