namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class RacingBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 3;

        private IAuthorizationService _authorizationService;
        private IRacingBetSettingDataAccess _dataAccess;
        private IRacingBetSettingService _service;

        [SetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IRacingBetSettingDataAccess>();
            _service = new RacingBetSettingService(_authorizationService, _dataAccess);

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            var isAllowSaToteCurrency = true;
            var betSettings = new List<BetSetting>()
            {
                new BetSetting()
            };
            _dataAccess.GetBetSetting(CustId, isAllowSaToteCurrency).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, isAllowSaToteCurrency);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            var commonBetSetting = new CommonBetSetting();
            var racingParam = new Dictionary<string, RacingBetSetting>()
            {
                ["0"] = new RacingBetSetting(CustId, commonBetSetting)
            };

            // Action
            var result = _service.UpdateBetSetting(LicenseeId, racingParam);

            // Assert
            _dataAccess.Received().UpdateBetSetting(Arg.Any<Dictionary<string, RacingBetSetting>>());
        }
    }
}