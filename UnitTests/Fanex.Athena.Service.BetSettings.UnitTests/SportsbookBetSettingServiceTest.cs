namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

    [TestFixture]
    public class SportsbookBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int CustId = 3;

        private IAuthorizationService _authorizationService;
        private ISportsbookBetSettingDataAccess _dataAccess;
        private ISportsbookBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<ISportsbookBetSettingDataAccess>();
            _service = new SportsbookBetSettingService(_authorizationService, _dataAccess);
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
            var esportSetting = EsportSettingType.NoneOffering;
            var betSettings = new List<BetSetting>()
            {
                new BetSetting()
            };
            _dataAccess.GetBetSetting(CustId, ref esportSetting).Returns(betSettings);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId);

            // Assert
            Assert.That(result.MainData.Count, Is.EqualTo(betSettings.Count));
        }

        [Test]
        public void UpdateBetSetting_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

            // Action
            var result = _service.UpdateBetSetting(new CommonBetSetting(), new Dictionary<string, object>());

            // Assert
            _dataAccess.Received().UpdateBetSetting(Arg.Any<CommonBetSetting>(), Arg.Any<Dictionary<string, object>>());
        }

        [Test]
        public void UpdateBetSetting_SettingParamIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);

            // Action
            var result = _service.UpdateBetSetting(null, new Dictionary<string, object>());

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. sportsBook is null"));
        }

        [Test]
        public void UpdateBetSetting_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);

            // Action
            var result = _service.UpdateBetSetting(new CommonBetSetting(), new Dictionary<string, object>());

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. Invalid custId."));
        }

        [Test]
        public void UpdateEsportStatus_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var isDisabled = true;

            // Action
            var result = _service.UpdateEsportStatus(new CommonBetSetting(), isDisabled);

            // Assert
            _dataAccess.Received().UpdateEsportStatus(Arg.Any<CommonBetSetting>(), isDisabled);
        }

        [Test]
        public void UpdateEsportStatus_SettingParamIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            var isDisabled = true;

            // Action
            var result = _service.UpdateEsportStatus(null, isDisabled);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo("Invalid parameter. sportsBook is null"));
        }
    }
}