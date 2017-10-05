namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using Base.UnitTests;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Logging;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Services;

    [TestFixture]
    public class OneAuthorizationControllerTest : BaseControllerHelperTest
    {
        private IMaintenanceService _maintenanceService;
        private IOneAuthenticationService _oneAuthenticationService;
        private ILogger _logger;
        private int _loggerCounnter;
        private OneAuthorizationController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerCounnter = 0;

            _maintenanceService = Substitute.For<IMaintenanceService>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _logger = Substitute.For<ILogger>();

            StubGetValueAppSettings("EnableLogForOneAuthorization", "0");
            _logger.When(x => x.Error(Arg.Any<string>())).Do(x => _loggerCounnter++);
            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);

            _controller = new OneAuthorizationController(_maintenanceService, _oneAuthenticationService, AppSettings, _logger)
            {
                ControllerContext = ControllerContext,
                Url = Url
            };
        }

        [TearDown]
        public void TearDown()
        {
            _maintenanceService = null;
            _logger = null;
            _controller.Dispose();
            _loggerCounnter = 0;
        }

        [Test]
        public void CreateSession_UnderMaintenanceState_RedirectToUMPage()
        {
            // Arrange
            _maintenanceService.IsUnderMaintenance().Returns(true);

            const string ExpectedUrl = "/Utility/UnderMaintenance?notReload=true";

            // Action
            var result = _controller.CreateSession(string.Empty);

            // Assert
            StringAssert.Contains(ExpectedUrl, result.Url);
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("   ")]
        public void CreateSession_InvalidToken_ReturnTokenRestrictionPage(string token)
        {
            // Arrange
            const string ExpectedUrl = "Utility/OneTokenRestriction";

            // Action
            var result = _controller.CreateSession(token);

            // Assert
            StringAssert.Contains(ExpectedUrl, result.Url);
        }

        [TestCase("ghskhgk9863096804396oisoie", OneAuthorizationErrorCode.TokenIsInValid)]
        public void CreateSession_NotAuthenticateToken_ReturnTokenRestrictionPage(string token, int expectedCode)
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
            const string Token = "GasgSDgS";
            const string ExpectedMsg = "token is null";

            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);
            StubGetValueAppSettings("EnableLogForOneAuthorization", "1");

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(ExpectedMsg)));
                Assert.That(_loggerCounnter, Is.EqualTo(1));
            });
        }

        [Test]
        public void CreateSession_NotAuthenticatedByBadToken_WriteLog()
        {
            // Arrange
            const string Token = "GasgSDgS";

            var tokenInfo = new OneAuthToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                VendorID = "localhost"
            };
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);

            StubGetValueAppSettings("EnableLogForOneAuthorization", "1");

            var expectedMsg = JsonConvert.SerializeObject(tokenInfo);
                    
            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(expectedMsg)));
                Assert.That(_loggerCounnter, Is.EqualTo(1));
            });
        }

        [Test]
        public void CreateSession_AuthenticatedTokenButWrongDomain_RedirectToRestrictionPage()
        {
            // Arrange
            const string Token = "GasgSDgS";
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => new OneAuthToken
            {
                SiteName = "spondemo",
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/LicenseeWinLossReport",
                Language = "en-US",
                UserID = "pgmAlex",
                VendorID = "localhost"
            });

            StubGetValueAppSettings("InternalSubDomain", "localhost");

            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));

            var expectedUrl = "Utility/OneTokenRestriction?code=" + OneAuthorizationErrorCode.LocationRestrictionMsg;

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }

        [TestCase("spondemo", "hLicenseeWinLossReport")]
        public void CreateSession_Authenticated_RedirectToActionLink(string siteName, string actionLink)
        {
            // Arrange
            const string Token = "GasgSDgS";
            var tokenInfo = new OneAuthToken
            {
                SiteName = siteName,
                UserSEQ = 1,
                ActionLink = actionLink,
                Language = "en-US",
                UserID = "pgmAlex",
                VendorID = "localhost"
            };
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);

            var resultInfo = new ResultInfo { Code = 0, Message = string.Empty, ExtendedData = actionLink };
            _oneAuthenticationService.SignIn(_controller.HttpContext, tokenInfo).Returns(resultInfo);

            StubGetValueAppSettings("InternalSubDomain", "alex.");

            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));
                       
            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(actionLink, result.Url);
        }

        [TestCase(500)]
        public void CreateSession_NotAuthenticated_RedirectToRestrictionPageWithErrorCode(int expectedErrorCode)
        {
            // Arrange
            const string Token = "GasgSDgS";

            var tokenInfo = new OneAuthToken
            {
                SiteName = "spondemo",
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/alexakak",
                Language = "en-US",
                UserID = "pgmAlex",
                VendorID = "localhost"
            };
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);

            var resultInfo = new ResultInfo { Code = expectedErrorCode, Message = "error" };
            _oneAuthenticationService.SignIn(_controller.HttpContext, tokenInfo).Returns(resultInfo);

            StubGetValueAppSettings("InternalSubDomain", "alex.");

            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));            
            
            var expectedUrl = "Utility/OneTokenRestriction?code=" + expectedErrorCode.ToString();

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }
    }
}