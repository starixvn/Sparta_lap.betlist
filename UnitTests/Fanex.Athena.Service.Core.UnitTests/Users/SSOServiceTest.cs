namespace Fanex.Athena.Service.Core.UnitTests.Users
{
    using System;
    using System.IO;
    using System.Web;
    using Core.Users;
    using Core.Users.Interfaces;
    using Core.Utility.Interfaces;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Logging;
    using Model.Core.MetaModel.Authorization;
    using NSubstitute;
    using NUnit.Framework;
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
            _httpContextService = Substitute.ForPartsOf<HttpContextService>();
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
                appSetting);
        }

        [Test]
        public void SSO_SSOKeyPathIsNotExists_RedirectToErrorPage()
        {
            // Arrange
            var service = CreateSSOService(null);

            // Action
            service.SSO();

            // Assert
            _httpContextService.Received().Redirect("~/deniedPath?code=2");
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
            _httpContextService.Received(2).Redirect("~/deniedPath?code=1");
        }

        [Test]
        public void SSO_PrivateKeyPathIsInvalid_RedirectErrorPage()
        {
            // Arrange
            var requestKey = FakeHelper.Create<string>();
            var privateKeyPath = string.Empty;

            var context = CreateHttpContext(requestKey);
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
                _httpContextService.Received(1).Redirect("~/deniedPath?code=1");
                _httpContextService.Received(1).Redirect("~/deniedPath?code=2");
            });
        }

        [Test]
        public void SSO_RequestedUserNameIsEmpty_RedirectErrorPage()
        {
            // Arrange
            var requestKey = FakeHelper.Create<string>();
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
            _httpContextService.Received(1).Redirect("~/deniedPath?code=1");
        }

        [Test]
        public void SSO_RequestedPasswordIsEmpty_RedirectErrorPage()
        {
            // Arrange
            var requestKey = FakeHelper.Create<string>();
            var privateKeyPath = FakeHelper.Create<string>();

            var context = CreateHttpContext(requestKey);
            HttpContext.Current = context;

            _httpContextService.MapPath(Arg.Any<string>()).Returns(privateKeyPath);
            _fileSystem.Exists(Arg.Any<string>()).Returns(true);

            var metaModel = new SsoMetaModel
            {
                UserName = FakeHelper.Create<string>(),
                LicenseeName = FakeHelper.Create<string>(),
                Password = string.Empty,
                LoginUrl = FakeHelper.Create<string>()
            };

            var decryptedQueryString = BuildQueryString(metaModel);
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);

            // Action
            _ssoService.SSO();

            // Assert
            _httpContextService.Received(1).Redirect("~/deniedPath?code=1");
        }

        private string BuildQueryString(SsoMetaModel model)
        => $"licname={model.LicenseeName}&username={model.UserName}&pwd={model.Password}&loginurl={model.LoginUrl}&lang={model.LanguageCode}&timestamp="
            + DateTime.Now.Ticks;

        private HttpContext CreateHttpContext(string requestKey)
        {
            var request = new HttpRequest(string.Empty, "http://tempuri.org", "key=" + requestKey);
            var response = new HttpResponse(new StringWriter());
            return new HttpContext(request, response);
        }
    }
}