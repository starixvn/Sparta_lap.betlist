namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.IO;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;
    using Core.Users;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SSOServiceTest
    {
        private IHttpContextService _httpContextService;
        private IAuthService _authService;
        private ISessionService _sessionService;
        private IRSAEncryptor _rsaEncryptor;
        private IFileSystem _fileSystem;
        private IFormsAuthenticationService _formsAuthenticationService;
        private ILogger _logger;
        private SSOService _ssoService;

        [SetUp]
        public void SetUp()
        {
            MockHttpContext();
            _httpContextService = Substitute.ForPartsOf<HttpContextService>();
            _authService = Substitute.For<IAuthService>();
            _sessionService = Substitute.For<ISessionService>();
            _rsaEncryptor = Substitute.For<IRSAEncryptor>();
            _fileSystem = Substitute.For<IFileSystem>();
            _formsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _logger = Substitute.For<ILogger>();
            var appSetting = Substitute.For<IAppSettings>();
            appSetting.GetValue("SSOUnAuthorizePath").Returns("deniedPath?code={0}");
            appSetting.GetValue("SSOKeyPath").Returns("keypath");

            _ssoService = Substitute.ForPartsOf<SSOService>(
                _httpContextService,
                _authService,
                _sessionService,
                _rsaEncryptor,
                _fileSystem,
                _formsAuthenticationService,
                _logger,
                appSetting);

            _httpContextService.When(x => x.Redirect(Arg.Any<string>())).DoNotCallBase();
            _fileSystem.Exists(Arg.Any<string>()).Returns(true);
        }

        [TearDown]
        public void TearDown()
        {
            _httpContextService = null;
            _sessionService = null;
            _rsaEncryptor = null;
            _fileSystem = null;
            _formsAuthenticationService = null;
            _ssoService = null;
        }

        [Test]
        public void SSO_EmptyPassword_RedirectToErrorPage()
        {
            // Arrange
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>())
                .Returns("licname=spondemo&username=user1");

            // Action
            _ssoService.SSO();

            // Assert
            _httpContextService.Received(1).Redirect("~/deniedPath?code=1");
        }

        [Test]
        public void SSO_WhenCall_RelevantMethodCalled()
        {
            // Arrange
            var timeSpan = DateTime.Now.Ticks;
            var decryptedQueryString = "licname=spondemo&username=user1&pwd=123&loginurl=null&lang=en-US&timestamp=" + timeSpan;
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);
            var onuserId = 12345;
            _authService.ValidateInternalUser(Arg.Any<User>(), "spondemo", timeSpan)
                .Returns(new ResultInfo<User>
                {
                    ExtendedData = new User
                    {
                        Username = "user1",
                        UserID = 1,
                        ServerId = 999,
                        Licensee = new Licensee
                        {
                            Id = 2
                        }
                    }
                });
            _authService.LoginOnUser(Arg.Any<User>(), HttpContext.Current.Session.SessionID, 999, HttpContext.Current.Request.UserHostAddress)
                .Returns(onuserId);

            // Action
            _ssoService.SSO();

            // Assert
            _sessionService.Received().CreateNewSession(1, onuserId, "user1", true, 2, "en-US", viewType: Model.Core.DataDomain.ClientViewType.Default);
            _formsAuthenticationService.Received().SetAuthCookie("spondemo_user1", Arg.Any<bool>());
        }

        [Test]
        public void SSO_LoginFailed_CallReleventMethods()
        {
            // Arrange
            const int TimeSpan = 1234;
            var decryptedQueryString = "licname=spondemo&username=user1&pwd=123&loginurl=null&lang=en-US&timestamp=" + TimeSpan;
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);
            var resultInfo = new ResultInfo<User>
            {
                Code = 1,
                Message = "Fail",
                ExtendedData = new User()
            };
            _authService.ValidateInternalUser(Arg.Any<User>(), "spondemo", TimeSpan).Returns(resultInfo);
            var expectedMessage = "Internal login failed." +
                                "\r\nCode: " + resultInfo.Code +
                                "\r\nMessage: " + resultInfo.Message +
                                "\r\nExtendedData: " +
                                "\r\nLicensee: spondemo" +
                                 "\r\nUserName: user1" +
                                 "\r\nTimeSpan: 1234" +
                                 "\r\nSiteOutUrl: null" +
                                 "\r\nLanguageCode: en-US";

            // Action
            _ssoService.SSO();

            // Assert
            _logger.Received(1).Info(expectedMessage);
            _httpContextService.Received(1).Redirect("~/deniedPath?code=1");
        }

        [Test]
        public void SSO_UserIsAuthenticatedButNotMatchWithCurrentUserName_SetAuthCookieIsNotCalled()
        {
            // Arrange
            var timeSpan = DateTime.Now.Ticks;
            var decryptedQueryString = "licname=spondemo&username=user2&pwd=123&loginurl=null&lang=abc&timestamp=" + timeSpan;
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);

            var userSession = new UserSession
            {
                UserId = 1
            };
            _sessionService.UserSession.Returns(userSession);

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };
            _sessionService.LicenseeSession.Returns(licenseeSession);

            var onuserId = 12345;
            _authService.ValidateInternalUser(Arg.Any<User>(), "spondemo", timeSpan)
                .Returns(new ResultInfo<User>
                {
                    ExtendedData = new User
                    {
                        Username = "user1",
                        UserID = 1,
                        ServerId = 999,
                        Licensee = new Licensee
                        {
                            Id = 2
                        }
                    }
                });
            _authService.LoginOnUser(Arg.Any<User>(), HttpContext.Current.Session.SessionID, 999, HttpContext.Current.Request.UserHostAddress)
                .Returns(onuserId);

            // Action
            _ssoService.SSO();

            // Assert
            _formsAuthenticationService.Received(1).SetAuthCookie("spondemo_user2", Arg.Any<bool>());
        }

        private static void MockHttpContext()
        {
            var queryString = "key=123";
            var request = new HttpRequest(string.Empty, "http://tempuri.org", queryString);
            var response = new HttpResponse(new StringWriter());
            var httpContext = new HttpContext(request, response);

            var sessionContainer = new HttpSessionStateContainer(
                "id",
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            httpContext.User = new GenericPrincipal(new GenericIdentity("user1"), new string[0]);
            HttpContext.Current = httpContext;
        }
    }
}