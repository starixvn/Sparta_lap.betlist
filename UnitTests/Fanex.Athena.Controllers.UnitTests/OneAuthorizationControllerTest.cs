using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.DTO.Common;
using Fanex.Athena.Service.Core.Users;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Logging;
using NSubstitute;
using NUnit.Framework;
using ONEbook.UM.Services;

namespace Fanex.Athena.Controllers.UnitTests
{
    [TestFixture]
    public class OneAuthorizationControllerTest : BaseControllerTest
    {
        private IUserService _userService;
        private IMaintenanceService _maintenanceService;
        private IOneAuthenticationService _oneAuthenticationService;
        private IAppSettings _appSetting;
        private ILogger _logger;
        private int loggerCounnter = 0;

        [OneTimeSetUp]
        public void Setup()
        {
            loggerCounnter = 0;
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void CreateSession_InvalidToken_ReturnTokenRestrictionPage(string token)
        {
            // Arrange
            var controller = FakeController();
            var expectedUrl = "Utility/OneTokenRestriction";

            // Action
            var result = controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [TestCase("ghskhgk9863096804396oisoie", "300")]
        public void CreateSession_NotAuthenticateToken_ReturnTokenRestrictionPage(string token, string expectedCode)
        {
            // Arrange
            var controller = FakeController();
            var expectedUrl = "Utility/OneTokenRestriction?code=" + expectedCode;
            _oneAuthenticationService.GetTokenInfo(token).Returns(x => null);
            // Action
            var result = controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [Test]
        public void CreateSession_NotAuthenticateToken_WriteLog()
        {
            // Arrange
            var token = "GasgSDgS";
            var controller = FakeController();
            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("1");
            _oneAuthenticationService.GetTokenInfo(token).Returns(x => null);
            // Action
            var result = controller.CreateSession(token);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(x => !string.IsNullOrEmpty(x)));
        }

        private OneAuthorizationController FakeController()
        {
            _userService = Substitute.For<IUserService>();
            _maintenanceService = Substitute.For<IMaintenanceService>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _appSetting = Substitute.For<IAppSettings>();
            _logger = Substitute.For<ILogger>();
            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("0");
            _logger.When(x => x.Error(Arg.Any<string>())).Do(x => loggerCounnter++); ;
            var controller = new OneAuthorizationController(_userService, _maintenanceService, _oneAuthenticationService, _appSetting, _logger);
            controller.ControllerContext = ControllerContext;

            controller.Url = this.Url;

            return controller;
        }
    }
}