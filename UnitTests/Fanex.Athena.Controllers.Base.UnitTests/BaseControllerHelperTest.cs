namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.RouterConfig;

    [TestFixture]
    public abstract class BaseControllerHelperTest
    {
        protected const string TestingRequestDomain = "http://unit-test-mode";

        protected const string IdentityName = "user";

        protected readonly UserSession DefaultUserSession = new UserSession
        {
            CurrencyList = new Dictionary<int, string> { { 1, "UUS" } },
            ViewType = Model.Core.DataDomain.ClientViewType.Default,
            UserId = 1,
            Language = "lan",
            UserName = "alex.vn",
            OnUserId = 999,
            FunctionIdList = new List<int>()
        };

        protected readonly LicenseeSession DefaultLicenseeSession = new LicenseeSession
        {
            LicenseeId = 100,
            LicenseeName = "Spondemo"
        };

        protected UrlHelper Url { get; set; }

        protected ControllerContext ControllerContext { get; set; }

        protected ILanguageService LangService { get; set; }

        protected ISessionService SessionService { get; set; }

        protected IAppSettings AppSettings { get; set; }

        protected IFileSystem FileSystem { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            LangService = Substitute.For<ILanguageService>();
            SessionService = Substitute.For<ISessionService>();
            AppSettings = Substitute.For<IAppSettings>();
            FileSystem = Substitute.For<IFileSystem>();
            FileSystem.ViewExists(string.Empty).ReturnsForAnyArgs(true);
            SetUpForContexts();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            Url = null;
            ControllerContext = null;
            LangService = null;
            SessionService = null;
            AppSettings = null;
        }

        protected void SetUpDefaultFakeData()
        {
            StubUserSessionData(DefaultUserSession);

            StubLicenseeSessionData(DefaultLicenseeSession);

            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            AppSettings.ClientViewType.Returns(DefaultUserSession.ViewType.ToString("d"));
        }

        protected void StubUserSessionData(UserSession fakeUserSession)
        {
            SessionService.UserSession.Returns(fakeUserSession);
        }

        protected void StubLicenseeSessionData(LicenseeSession fakeLicenseeSession)
        {
            SessionService.LicenseeSession.Returns(fakeLicenseeSession);
        }

        protected void StubGetValueAppSettings(string valueName, string fakeValue)
        {
            AppSettings.GetValue(valueName).Returns(fakeValue);
        }

        private void SetUpForContexts()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes, false);

            var fakeIdentity = new GenericIdentity(IdentityName);
            var principal = new GenericPrincipal(fakeIdentity, null);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.ApplicationPath.Returns("/");
            httpContext.Request.Url.Returns(new Uri(TestingRequestDomain));
            httpContext.Request.Cookies.Returns(new HttpCookieCollection());
            httpContext.Response.Cookies.Returns(new HttpCookieCollection());
            httpContext.Server.MapPath(Arg.Any<string>()).Returns(x => (string)x[0]);
            httpContext.User.Returns(principal);

            ControllerContext = Substitute.For<ControllerContext>();
            ControllerContext.HttpContext = httpContext;

            var requestContext = new RequestContext(httpContext, new RouteData());

            Url = new UrlHelper(requestContext, routes);
        }
    }
}