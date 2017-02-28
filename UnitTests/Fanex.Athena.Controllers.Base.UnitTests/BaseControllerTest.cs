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
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.RouterConfig;

    [TestFixture]
    public class BaseControllerTest
    {
        protected const string TestingRequestUrl = "http://unit-test-mode";

        protected const string SesssionUserName = "alex.vn";

        protected const int SessionLicenseeId = 100;

        protected const int SessionUserId = 1;

        protected const int SessionOnUserId = 999;

        protected UrlHelper Url { get; set; }

        protected ControllerContext ControllerContext { get; set; }

        protected ILanguageService LangService { get; set; }

        protected ISessionService SessionService { get; set; }

        protected IAppSettings AppSettings { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes, false);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.ApplicationPath.Returns("/");
            httpContext.Request.Url.Returns(new Uri(TestingRequestUrl));
            httpContext.Request.Cookies.Returns(new HttpCookieCollection());
            httpContext.Response.Cookies.Returns(new HttpCookieCollection());
            httpContext.Server.MapPath(Arg.Any<string>()).Returns(x => (string)x[0]);
            var fakeIdentity = new GenericIdentity("User");
            var principal = new GenericPrincipal(fakeIdentity, null);

            httpContext.User.Returns(principal);

            ControllerContext = Substitute.For<ControllerContext>();
            ControllerContext.HttpContext = httpContext;

            var requestContext = new RequestContext(httpContext, new RouteData());

            Url = new UrlHelper(requestContext, routes);
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

        protected void FakeBaseControllerDependency()
        {
            LangService = Substitute.For<ILanguageService>();

            SessionService = Substitute.For<ISessionService>();
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { 1, "UUS" } },
                ViewType = ClientViewType.Default,
                UserId = SessionUserId,
                Language = "en-Us",
                UserName = SesssionUserName,
                OnUserId = SessionOnUserId,
                FunctionIdList = new List<int>()
            });

            SessionService.LicenseeSession.Returns(new LicenseeSession
            {
                CurrencyName = new List<string> { "UUS" },
                LicenseeId = SessionLicenseeId,
                LicenseeName = "Spondemo",
                LicenseeType = new List<string>()
            });

            AppSettings = Substitute.For<IAppSettings>();

            AppSettings.ClientViewType.Returns("1");
        }
    }
}