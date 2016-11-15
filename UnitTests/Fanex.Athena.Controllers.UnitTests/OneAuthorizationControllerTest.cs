namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Logging;
    using Model.Core.DTO.Common;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Services;

    [TestFixture]
    public class OneAuthorizationControllerTest : BaseControllerTest
    {
        private IUserService _userService;
        private IMaintenanceService _maintenanceService;
        private IOneAuthenticationService _oneAuthenticationService;
        private IAppSettings _appSetting;
        private ILogger _logger;
        private int _loggerCounnter = 0;
        private OneAuthorizationController _controller = null;

        [SetUp]
        public void Setup()
        {
            _loggerCounnter = 0;

            _userService = Substitute.For<IUserService>();
            _maintenanceService = Substitute.For<IMaintenanceService>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _appSetting = Substitute.For<IAppSettings>();
            _logger = Substitute.For<ILogger>();

            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("0");
            _logger.When(x => x.Error(Arg.Any<string>())).Do(x => _loggerCounnter++);
            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);

            _controller = new OneAuthorizationController(_userService, _maintenanceService, _oneAuthenticationService, _appSetting, _logger);
            _controller.ControllerContext = ControllerContext;

            _controller.Url = this.Url;
        }

        [TearDown]
        public void TearDown()
        {
            _userService = null;
            _maintenanceService = null;
            _appSetting = null;
            _logger = null;
            _controller = null;
            _loggerCounnter = 0;
        }

        [Test]
        public void CreateSession_UnderMaintenanceState_RedirectToUMPage()
        {
            // Arrange
            var token = "ghskhgk9863096804396oisoie";
            _maintenanceService.IsUnderMaintenance().Returns(true);

            var expectedUrl = "/Utility/UnderMaintenance?notReload=true";

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void CreateSession_InvalidToken_ReturnTokenRestrictionPage(string token)
        {
            // Arrange
            var expectedUrl = "Utility/OneTokenRestriction";

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [TestCase("ghskhgk9863096804396oisoie", "300")]
        public void CreateSession_NotAuthenticateToken_ReturnTokenRestrictionPage(string token, string expectedCode)
        {
            // Arrange
            var expectedUrl = "Utility/OneTokenRestriction?code=" + expectedCode;

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [Test]
        public void CreateSession_NotAuthenticatedByNullToken_WriteLog()
        {
            // Arrange
            var token = "GasgSDgS";
            var expectedMsg = "token is null";
            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("1");
            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(expectedMsg)));
        }

        [Test]
        public void CreateSession_NotAuthenticatedByBadToken_WriteLog()
        {
            // Arrange
            var token = "GasgSDgS";
            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("1");
            var tokenInfo = new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                VendorID = "localhost"
            };

            var expectedMsg = JsonConvert.SerializeObject(tokenInfo);

            _oneAuthenticationService.GetTokenInfo(token).Returns(x => tokenInfo);

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(expectedMsg)));
        }

        [Test]
        public void CreateSession_AuthenticatedTokenButWrongDomain_RedirectToRestrictionPage()
        {
            // Arrange
            var token = "GasgSDgS";
            _appSetting.GetValue(Arg.Is<string>("internaldomain")).Returns(x => "localhost");
            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));
            _oneAuthenticationService.GetTokenInfo(token).Returns(x => new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/LicenseeWinLossReport",
                Language = "en-US",
                UserName = "pgmAlex",
                VendorID = "localhost"
            });
            var expectedUrl = "Utility/OneTokenRestriction?code=400";

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }
    }
}