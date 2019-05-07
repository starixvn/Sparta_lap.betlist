namespace Fanex.Athena.Service.BetSettings.UnitTests
{
    using System.Threading.Tasks;
    using BetSettings;
    using BetSettings.Interfaces;
    using DataAccess.BetSettings.Interfaces;
    using Model.BetSettings.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using StatelessCore.Interfaces;

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
        public async Task GetViewModelAsync_WhenCall_ReturnDataFromDataAcccess()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const int ProductId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsEnabled = true,
                IsLineEnabled = true
            };

            _dataAccess.GetBetSettingAsync(CustId, ProductId).Returns(Task.FromResult(betSetting));

            // Action
            var result = await _service.GetViewModelAsync(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result.IsEnabled, Is.EqualTo(betSetting.IsEnabled));
            Assert.That(result.IsLineEnabled, Is.EqualTo(betSetting.IsLineEnabled));
        }

        [Test]
        public async Task GetViewModelAsync_HaveNoPermission_ReturnNull()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            const int ProductId = 100;

            // Action
            var result = await _service.GetViewModelAsync(LicenseeId, CustId, ProductId);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_CallDataAccessWithCorrectParameters()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const int ProductId = 100;

            var betSetting = new ThirdPartyBetSetting
            {
                IsLineEnabled = true
            };
            _dataAccess.GetBetSettingAsync(CustId, ProductId).Returns(Task.FromResult(betSetting));

            // Action
            var result = await _service.UpdateStatusAsync(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            await _dataAccess.Received(1).UpdateStatusAsync(ProductId, UserId, CustId, Language, "ip", true);
        }

        [Test]
        public async Task UpdateStatusAsync_HaveNoPermission_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(false);
            const int ProductId = 100;

            // Action
            var result = await _service.UpdateStatusAsync(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }

        [Test]
        public async Task UpdateStatusAsync_IsLineEnabledInSettingIsNull_ReturnCorrectErrorResult()
        {
            // Arrange
            _authorizationService.HasPermissionOnData(Arg.Any<int>(), Arg.Any<int>(), Arg.Any<long>()).Returns(true);
            const int ProductId = 100;
            var betSetting = new ThirdPartyBetSetting
            {
                IsLineEnabled = false
            };
            _dataAccess.GetBetSettingAsync(CustId, ProductId).Returns(Task.FromResult(betSetting));

            // Action
            var result = await _service.UpdateStatusAsync(LicenseeId, ProductId, UserId, CustId, Language, "ip", true);

            // Assert
            Assert.That(result.Code, Is.EqualTo(1));
            Assert.That(result.Message, Is.EqualTo(ViewResources.AccessDeniedMsg));
        }
    }
}