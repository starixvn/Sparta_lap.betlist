namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Extension.Utility;
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

        protected const string SesssionUserName = "alex.vn";

        protected const int SessionLicenseeId = 100;

        protected const int SessionUserId = 1;

        protected const int SessionOnUserId = 999;

        protected const string IdentityName = "user";

        protected const string LicenseeName = "Spondemo";

        protected const string Language = "en-Us";

        protected const string ClientViewType = "1";

        protected readonly Dictionary<int, string> CurrencyList = new Dictionary<int, string> { { 1, "UUS" } };

        protected UrlHelper Url { get; set; }

        protected ControllerContext ControllerContext { get; set; }

        protected ILanguageService LangService { get; set; }

        protected ISessionService SessionService { get; set; }

        protected IAppSettings AppSettings { get; set; }

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            LangService = Substitute.For<ILanguageService>();
            SessionService = Substitute.For<ISessionService>();
            AppSettings = Substitute.For<IAppSettings>();

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

        protected void MockDefaultData()
        {
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = CurrencyList,
                ViewType = Model.Core.DataDomain.ClientViewType.Default,
                UserId = SessionUserId,
                Language = Language,
                UserName = SesssionUserName,
                OnUserId = SessionOnUserId,
                FunctionIdList = new List<int>()
            });

            SessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeId = SessionLicenseeId,
                LicenseeName = LicenseeName
            });

            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            AppSettings.ClientViewType.Returns(ClientViewType);
        }

        protected void MockGetValueAppSettings(string valueName, string value)
        {
            AppSettings.GetValue(valueName).Returns(value);
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