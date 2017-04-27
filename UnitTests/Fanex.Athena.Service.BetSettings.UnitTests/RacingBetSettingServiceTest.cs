namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Collections.Generic;
    using BetSettings;
    using BetSettings.Interfaces;
    using Core.Exception;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RacingBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 3;

        private IAuthorizationService _authorizationService;
        private IRacingBetSettingDataAccess _dataAccess;
        private IRacingBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IRacingBetSettingDataAccess>();
            _service = new RacingBetSettingService(_authorizationService, _dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _authorizationService = null;
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetViewModel_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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

        [Test]
        public void UpdateBetSetting_RacingParamIsNull_ThrowException()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var commonBetSetting = new CommonBetSetting();
            Dictionary<string, RacingBetSetting> racingParam = null;

            // Action
            TestDelegate result = () => _service.UpdateBetSetting(LicenseeId, racingParam);

            // Assert
            Assert.Throws<ServiceException>(result, "Invalid parameter. racingParam is null", "BetSetting");
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ThrowException()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            var commonBetSetting = new CommonBetSetting();
            var racingParam = new Dictionary<string, RacingBetSetting>()
            {
                ["0"] = new RacingBetSetting(CustId, commonBetSetting)
            };

            // Action
            TestDelegate result = () => _service.UpdateBetSetting(LicenseeId, racingParam);

            // Assert
            Assert.Throws<ServiceException>(result, "Invalid parameter. Invalid custId.", "BetSetting");
        }
    }
}