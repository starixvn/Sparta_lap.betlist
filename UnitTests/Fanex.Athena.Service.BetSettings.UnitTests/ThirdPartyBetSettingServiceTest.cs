namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using BetSettings;
    using BetSettings.Interfaces;
    using Core.Users.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    [TestFixture]
    public class ThirdPartyBetSettingServiceTest
    {
        private const int LicenseeId = 1;
        private const int UserId = 2;
        private const int CustId = 3;
        private const string Language = "en-US";

        private IAuthorizationService _authorizationService;
        private IThirdPartyBetSettingDataAccess _dataAccess;
        private IThirdPartyBetSettingService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _authorizationService = Substitute.For<IAuthorizationService>();
            _dataAccess = Substitute.For<IThirdPartyBetSettingDataAccess>();
            _service = new ThirdPartyBetSettingService(_authorizationService, _dataAccess);
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
            const int ProductId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = true
            };

            _dataAccess.GetBetSetting(CustId, ProductId).Returns(betSetting);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(betSetting.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(betSetting.IsLineEnabled));
        }

        [Test]
        public void GetViewModel_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            const int ProductId = 100;

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void UpdateStatus_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const int ProductId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true
            };
            _dataAccess.GetBetSetting(CustId, ProductId).Returns(betSetting);

            // Action
            var result = _service.UpdateStatus(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            _dataAccess.Received().UpdateStatus(ProductId, UserId, CustId, Language, "ip", true);
        }

        [Test]
        public void UpdateStatus_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            const int ProductId = 100;

            // Action
            var result = _service.UpdateStatus(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }

        [Test]
        public void UpdateStatus_IsLineEnabledInSettingIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const int ProductId = 100;
            var betSetting = new ThirdPartyBetSetting
            {
                IsLineEnabled = false
            };
            _dataAccess.GetBetSetting(CustId, ProductId).Returns(betSetting);

            // Action
            var result = _service.UpdateStatus(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}