namespace Fanex.Athena.Service.Core.UnitTests.Common
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
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SSOServiceTest
    {
        private IHttpContextService _httpContextService;
        private IUserService _userService;
        private ISessionService _sessionService;
        private IRSAEncryptor _rsaEncryptor;
        private IFileSystem _fileSystem;
        private IFormsAuthenticationService _formsAuthenticationService;
        private SSOService _ssoService;

        [OneTimeSetUp]
        public void SetUp()
        {
            MockHttpContext();
            _httpContextService = Substitute.ForPartsOf<HttpContextService>();
            _userService = Substitute.For<IUserService>();
            _sessionService = Substitute.For<ISessionService>();
            _rsaEncryptor = Substitute.For<IRSAEncryptor>();
            _fileSystem = Substitute.For<IFileSystem>();
            _formsAuthenticationService = Substitute.For<IFormsAuthenticationService>();
            _ssoService = Substitute.ForPartsOf<SSOService>(
                _httpContextService,
                _userService,
                _sessionService,
                _rsaEncryptor,
                _fileSystem,
                _formsAuthenticationService,
                "deniedPath",
                "keypath");

            _httpContextService.When(x => x.Redirect(Arg.Any<string>())).DoNotCallBase();
            _fileSystem.Exists(Arg.Any<string>()).Returns(true);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpContextService = null;
            _userService = null;
            _sessionService = null;
            _rsaEncryptor = null;
            _fileSystem = null;
            _formsAuthenticationService = null;
            _ssoService = null;
        }

        [Test]
        public void SSO_WhenCall_RelevantMethodCalled()
        {
            // Arrange
            var timeSpan = DateTime.Now.Ticks;
            var decryptedQueryString = "licname=spondemo&username=user1&pwd=123&loginurl=null&lang=en-US&timestamp=" + timeSpan;
            _rsaEncryptor.DecryptString(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<string>()).Returns(decryptedQueryString);
            IResultInfo resultInfo;
            var onuserId = 12345;
            _userService.ValidateInternalUser(Arg.Any<User>(), "spondemo", timeSpan, out resultInfo)
                .Returns(x =>
                {
                    x[3] = new ResultInfo();
                    return new User
                    {
                        Username = "user1",
                        Id = 1,
                        ServerId = 999,
                        Licensee = new Licensee
                        {
                            Id = 2
                        }
                    };
                });
            _userService.LoginOnUser(Arg.Any<User>(), HttpContext.Current.Session.SessionID, 999, HttpContext.Current.Request.UserHostAddress)
                .Returns(onuserId);

            // Action
            _ssoService.SSO();

            // Assert
            _sessionService.Received().CreateNewSession(1, onuserId, "user1", true, 2, "en-US");
            _formsAuthenticationService.Received().SetAuthCookie("user1", Arg.Any<bool>());
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