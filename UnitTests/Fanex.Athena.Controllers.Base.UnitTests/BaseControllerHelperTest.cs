namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Helpers;
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

        protected IClientViewHelper ClientViewHelper { get; set; }

        protected IProductService ProductService { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            LangService = Substitute.For<ILanguageService>();
            SessionService = Substitute.For<ISessionService>();
            AppSettings = Substitute.For<IAppSettings>();
            ClientViewHelper = Substitute.For<IClientViewHelper>();
            ClientViewHelper.GetViewPath(Arg.Any<string>(), Arg.Any<ClientViewType>()).Returns(x => new ViewByOption
            {
                IsNavigationView = false,
                ViewPath = (string)x[0]
            });
            ProductService = Substitute.For<IProductService>();
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
            ClientViewHelper = null;
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

            var httpBaseContext = Substitute.For<HttpContextBase>();
            httpBaseContext.Request.ApplicationPath.Returns("/");
            httpBaseContext.Request.Url.Returns(new Uri(TestingRequestDomain));
            httpBaseContext.Request.Cookies.Returns(new HttpCookieCollection());
            httpBaseContext.Response.Cookies.Returns(new HttpCookieCollection());
            httpBaseContext.Server.MapPath(Arg.Any<string>()).Returns(x => (string)x[0]);
            httpBaseContext.User.Returns(principal);

            var requestContext = new RequestContext(httpBaseContext, new RouteData());

            ControllerContext = Substitute.For<ControllerContext>();
            ControllerContext.HttpContext = httpBaseContext;

            Url = new UrlHelper(requestContext, routes);
        }
    }
}