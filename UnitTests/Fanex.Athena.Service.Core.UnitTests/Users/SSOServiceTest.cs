namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.IO;
    using System.Security.Principal;
    using System.Web;
    using System.Web.SessionState;
    using Core.Common.Interfaces;
    using Core.Users;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using Model.Core.MetaModel.Authorization;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Enum;
    using Session.Model;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SSOServiceTest
    {
        private const string FakeSSOUnAuthorizePath = "deniedPath?code={0}";
        private const string FakeSSOKeyPath = "keypath";

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
            _httpContextService = Substitute.For<IHttpContextService>();
            _authService = Substitute.For<IAuthService>();
            _sessionService = Substitute.For<ISessionService>();
            _rsaEncryptor = Substitute.For<IRSAEncryptor>();
            _fileSystem = Substitute.For<IFileSystem>();
            _formsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _logger = Substitute.For<ILogger>();

            _ssoService = CreateSSOService();

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

        private SSOService CreateSSOService(string ssoKeyPath = FakeSSOKeyPath)
        {
            var appSetting = Substitute.For<IAppSettings>();
            appSetting.GetValue("SSOUnAuthorizePath").Returns(FakeSSOUnAuthorizePath);
            appSetting.GetValue("SSOKeyPath").Returns(ssoKeyPath);

            return Substitute.ForPartsOf<SSOService>(
                _httpContextService,
                _authService,
                _sessionService,
                _rsaEncryptor,
                _fileSystem,
                _formsAuthenticationService,
                _logger,
                appSetting,
                Substitute.For<ILanguageService>());
        }

        [Test]
        public void SSO_SSOKeyPathIsNotExists_RedirectToErrorPage()
        {
            // Arrange
            var service = CreateSSOService(null);

            // Action
            service.SSO();

            // Assert
            _httpContextService.Received().Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 2));
        }

        [Test]
        public void SSO_RequestKeyIsInvalid_RedirectErrorPage()
        {
            // Arrange
            var requestKey = string.Empty;
            var privateKeyPath = FakeHelper.Create<string>();

            var context = CreateHttpContext(requestKey);
            HttpContext.Current = context;

            _httpContextService.MapPath(Arg.Any<string>()).Returns(privateKeyPath);
            _fileSystem.Exists(Arg.Any<string>()).Returns(true);

            var metaModel = new SsoMetaModel
            {
                UserName = string.Empty,
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>()
            };

            var decryptedQueryString = BuildQueryString(metaModel);
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);

            // Action
            _ssoService.SSO();

            // Assert
            _httpContextService.Received(2).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 1));
        }

        [Test]
        public void SSO_PrivateKeyPathIsInvalid_RedirectErrorPage()
        {
            // Arrange
            var requestKey = FakeHelper.Create<string>();
            var privateKeyPath = string.Empty;

            var context = CreateHttpContext("key=" + requestKey);
            HttpContext.Current = context;

            _httpContextService.MapPath(Arg.Any<string>()).Returns(privateKeyPath);
            _fileSystem.Exists(Arg.Any<string>()).Returns(false);

            var metaModel = new SsoMetaModel
            {
                UserName = string.Empty,
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>()
            };

            var decryptedQueryString = BuildQueryString(metaModel);
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);

            // Action
            _ssoService.SSO();

            // Assert
            Assert.Multiple(() =>
            {
                _httpContextService.Received(1).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 1));
                _httpContextService.Received(1).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 2));
            });
        }

        [Test]
        public void SSO_RequestedUserNameIsEmpty_RedirectErrorPage()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = string.Empty,
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>()
            };

            SetUpForParseGetQueryParams(metaModel);

            // Action
            _ssoService.SSO();

            // Assert
            _httpContextService.Received(1).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 1));
        }

        [Test]
        public void SSO_RequestedPasswordIsEmpty_RedirectErrorPage()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = string.Empty,
                LoginUrl = FakeHelper.Create<string>()
            };

            SetUpForParseGetQueryParams(metaModel);

            // Action
            _ssoService.SSO();

            // Assert
            _httpContextService.Received(1).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, 1));
        }

        [Test]
        public void SSO_IsNotUserAuthenticated_InternalLogin()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>(),
                TimeStamp = DateTime.Now.Ticks
            };

            SetUpForParseGetQueryParams(metaModel);

            var context = HttpContext.Current;
            var sessionContainer = new HttpSessionStateContainer(
                FakeHelper.Create<string>(),
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(context, sessionContainer);

            _httpContextService.Request.Returns(context.Request);
            _httpContextService.Session.Returns(context.Session);
            _sessionService.UserSession.Returns(new UserSession { UserId = 0 });
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = 0 });

            var resultInfo = new ResultInfo<User> { Code = Math.Abs(FakeHelper.Create<int>()) + 1, Message = FakeHelper.Create<string>() };
            _authService.ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp).Returns(resultInfo);

            // Action
            _ssoService.SSO();

            // Assert
            _authService.Received(1).ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp);
        }

        [Test]
        public void SSO_InvalidIdentityFailed_InternalLogin()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>(),
                TimeStamp = DateTime.Now.Ticks,
                LanguageCode = "en-US"
            };

            SetUpForParseGetQueryParams(metaModel);

            var context = HttpContext.Current;
            context.User = new GenericPrincipal(new GenericIdentity("Test User"), new string[0]);
            var sessionContainer = new HttpSessionStateContainer(
                FakeHelper.Create<string>(),
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(context, sessionContainer);

            _httpContextService.Request.Returns(context.Request);
            _httpContextService.Session.Returns(context.Session);
            _sessionService.UserSession.Returns(new UserSession { UserId = Math.Abs(FakeHelper.Create<int>()) + 1 });
            _sessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = Math.Abs(FakeHelper.Create<int>()) + 1 });

            var user = new User { Licensee = new Licensee() };
            var resultInfo = new ResultInfo<User> { Code = 0, ExtendedData = user };
            _authService.ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp).Returns(resultInfo);

            var onUserId = FakeHelper.Create<int>();
            _authService.LoginOnUser(user, context.Session.SessionID, user.ServerId, context.Request.UserHostAddress).Returns(onUserId);

            // Action
            _ssoService.SSO();

            // Assert
            _authService.Received(1).ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp);
        }

        [Test]
        public void SSO_InternalLoginFailed_LogFailedLoginAndRedirectToErrorPage()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>(),
                TimeStamp = DateTime.Now.Ticks
            };

            SetUpForParseGetQueryParams(metaModel);

            var context = HttpContext.Current;
            var sessionContainer = new HttpSessionStateContainer(
                FakeHelper.Create<string>(),
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(context, sessionContainer);

            _httpContextService.Request.Returns(context.Request);
            _httpContextService.Session.Returns(context.Session);

            var resultInfo = new ResultInfo<User> { Code = Math.Abs(FakeHelper.Create<int>()) + 1, Message = FakeHelper.Create<string>() };
            _authService.ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp).Returns(resultInfo);

            // Action
            _ssoService.SSO();

            // Assert
            Assert.Multiple(() =>
            {
                _logger.Received().Info(Arg.Is<string>(x => x.StartsWith("Internal login failed.")));
                _httpContextService.Received(1).Redirect("~/" + string.Format(FakeSSOUnAuthorizePath, resultInfo.Code));
            });
        }

        [Test]
        public void SSO_InternalLoginSuccessfully_CallRelevantMethodsRedirectToHomePage()
        {
            // Arrange
            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = FakeHelper.Create<string>(),
                LoginUrl = FakeHelper.Create<string>(),
                TimeStamp = DateTime.Now.Ticks,
                LanguageCode = "en-US"
            };

            SetUpForParseGetQueryParams(metaModel);

            var context = HttpContext.Current;
            var sessionContainer = new HttpSessionStateContainer(
                FakeHelper.Create<string>(),
                new SessionStateItemCollection(),
                new HttpStaticObjectsCollection(),
                10,
                true,
                HttpCookieMode.AutoDetect,
                SessionStateMode.InProc,
                false);
            SessionStateUtility.AddHttpSessionStateToContext(context, sessionContainer);

            _httpContextService.Request.Returns(context.Request);
            _httpContextService.Session.Returns(context.Session);

            var user = new User { Licensee = new Licensee() };
            var resultInfo = new ResultInfo<User> { Code = 0, ExtendedData = user };
            _authService.ValidateInternalUser(Arg.Any<User>(), metaModel.LicenseeName, metaModel.TimeStamp).Returns(resultInfo);

            var onUserId = FakeHelper.Create<int>();
            _authService.LoginOnUser(user, context.Session.SessionID, user.ServerId, context.Request.UserHostAddress).Returns(onUserId);

            // Action
            _ssoService.SSO();

            // Assert
            Assert.Multiple(() =>
            {
                _authService.Received(1).LoginOnUser(user, context.Session.SessionID, user.ServerId, context.Request.UserHostAddress);
                _sessionService.Received(1).CreateNewSession(user.UserID, onUserId, user.Username, true, user.Licensee.Id, metaModel.LanguageCode, ClientViewType.Default);
                _formsAuthenticationService.Received(1).SetAuthCookie(metaModel.IdentityName, false);
                _httpContextService.Received(1).Redirect("~/Home/Index");
            });
        }

        private string BuildQueryString(SsoMetaModel model)
        => $"licname={model.LicenseeName}&username={model.UserName}&pwd={model.Password}"
            + $"&loginurl={model.LoginUrl}&lang={model.LanguageCode}&timestamp={model.TimeStamp}";

        private HttpContext CreateHttpContext(string requestKey)
        {
            var request = new HttpRequest(string.Empty, "http://tempuri.org", requestKey);
            var response = new HttpResponse(new StringWriter());
            return new HttpContext(request, response);
        }

        private void SetUpForParseGetQueryParams(SsoMetaModel metaModel)
        {
            var requestKey = FakeHelper.Create<string>();
            var privateKeyPath = FakeHelper.Create<string>();

            var context = CreateHttpContext("key=" + requestKey);
            HttpContext.Current = context;

            _httpContextService.MapPath(Arg.Any<string>()).Returns(privateKeyPath);
            _fileSystem.Exists(Arg.Any<string>()).Returns(true);

            var decryptedQueryString = BuildQueryString(metaModel);
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);
        }
    }
}