namespace Fanex.Athena.Controllers.UnitTests
{
    using System;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Fanex.Logging;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using ONEbook.UM.Services;

    [TestFixture]
    public class OneAuthorizationControllerTest : BaseControllerTest
    {
        private IMaintenanceService _maintenanceService;
        private IOneAuthenticationService _oneAuthenticationService;
        private IAppSettings _appSetting;
        private ILogger _logger;
        private int _loggerCounnter;
        private OneAuthorizationController _controller;

        [SetUp]
        public void Setup()
        {
            _loggerCounnter = 0;

            _maintenanceService = Substitute.For<IMaintenanceService>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _appSetting = Substitute.For<IAppSettings>();
            _logger = Substitute.For<ILogger>();

            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("0");
            _logger.When(x => x.Error(Arg.Any<string>())).Do(x => _loggerCounnter++);
            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);

            _controller = new OneAuthorizationController(_maintenanceService, _oneAuthenticationService, _appSetting, _logger)
            {
                ControllerContext = ControllerContext,

                Url = this.Url
            };
        }

        [TearDown]
        public void TearDown()
        {
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
            const string Token = "ghskhgk9863096804396oisoie";
            _maintenanceService.IsUnderMaintenance().Returns(true);

            const string ExpectedUrl = "/Utility/UnderMaintenance?notReload=true";

            // Action
            var result = _controller.CreateSession(Token);

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
            const string Token = "GasgSDgS";
            const string ExpectedMsg = "token is null";

            _oneAuthenticationService.GetTokenInfo(Arg.Any<string>()).Returns(x => null);
            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("1");

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(ExpectedMsg)));
            Assert.AreEqual(1, _loggerCounnter);
        }

        [Test]
        public void CreateSession_NotAuthenticatedByBadToken_WriteLog()
        {
            // Arrange
            const string Token = "GasgSDgS";
            var tokenInfo = new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                VendorID = "localhost"
            };

            var expectedMsg = JsonConvert.SerializeObject(tokenInfo);

            _appSetting.GetValue(Arg.Is<string>("EnableLogForOneAuthorization")).Returns("1");

            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            _logger.Received(1).Error(Arg.Is<string>(x => x.Contains(expectedMsg)));
            Assert.AreEqual(1, _loggerCounnter);
        }

        [Test]
        public void CreateSession_AuthenticatedTokenButWrongDomain_RedirectToRestrictionPage()
        {
            // Arrange
            const string Token = "GasgSDgS";
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/LicenseeWinLossReport",
                Language = "en-US",
                UserName = "pgmAlex",
                VendorID = "localhost"
            });
            const string ExpectedUrl = "Utility/OneTokenRestriction?code=" + OneAuthorizationErrorCode.LocationRestrictionMsg;
            _appSetting.GetValue(Arg.Is<string>("internaldomain")).Returns(x => "localhost");
            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(ExpectedUrl, result.Url);
        }

        [TestCase("spondemoasfs")]
        public void CreateSession_NotFoundLicensee_RedirectToRestrictionPage(string siteName)
        {
            // Arrange
            const string Token = "GasgSDgS";
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = siteName,
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/LicenseeWinLossReport",
                Language = "en-US",
                UserName = "pgmAlex",
                VendorID = "localhost"
            });
            const string ExpectedUrl = "Utility/OneTokenRestriction?code=" + OneAuthorizationErrorCode.LicenseeIsNotExisted;

            _appSetting.GetValue(Arg.Is<string>("internaldomain")).Returns(x => "alex.");
            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(ExpectedUrl, result.Url);
        }

        [TestCase("spondemo", "hLicenseeWinLossReport")]
        public void CreateSession_Authenticated_RedirectToActionLink(string siteName, string actionLink)
        {
            // Arrange
            const string Token = "GasgSDgS";
            var tokenInfo = new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = siteName,
                UserSEQ = 1,
                ActionLink = actionLink,
                Language = "en-US",
                UserName = "pgmAlex",
                VendorID = "localhost"
            };
            var resultInfo = new ResultInfo { Code = 0, Message = string.Empty, ExtendedData = actionLink };

            _appSetting.GetValue(Arg.Is<string>("internaldomain")).Returns(x => "alex.");
            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);
            _oneAuthenticationService.SignIn(_controller.HttpContext, tokenInfo).Returns(resultInfo);

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

            var tokenInfo = new OneAuthenticationToken
            {
                UserID = "1",
                SiteName = "spondemo",
                UserSEQ = 1,
                ActionLink = "http://localhost:6985/alexakak",
                Language = "en-US",
                UserName = "pgmAlex",
                VendorID = "localhost"
            };
            var resultInfo = new ResultInfo { Code = expectedErrorCode, Message = "error" };
            _appSetting.GetValue(Arg.Is<string>("internaldomain")).Returns(x => "alex.");
            _controller.Request.Url.Returns(x => new Uri("http://localhost:6985/"));
            _oneAuthenticationService.GetTokenInfo(Token).Returns(x => tokenInfo);
            
            _oneAuthenticationService.SignIn(_controller.HttpContext, tokenInfo).Returns(resultInfo);
            var expectedUrl = "Utility/OneTokenRestriction?code=" + expectedErrorCode.ToString();

            // Action
            var result = _controller.CreateSession(Token);

            // Assert
            StringAssert.Contains(expectedUrl, result.Url);
        }
    }
}