namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Users.Interfaces;

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

            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
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
            const int productId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = true
            };

            _dataAccess.GetBetSetting(CustId, productId).Returns(betSetting);

            // Action
            var result = _service.GetViewModel(LicenseeId, CustId, productId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(betSetting.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(betSetting.IsLineEnabled));
        }

        [Test]
        public void UpdateStatus_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange

            const int productId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true
            };
            _dataAccess.GetBetSetting(CustId, productId).Returns(betSetting);

            // Action
            var result = _service.UpdateStatus(LicenseeId, productId, UserId, CustId, Language, "ip", true);

            // Assert
            _dataAccess.Received().UpdateStatus(productId, UserId, CustId, Language, "ip", true);
        }
    }
}