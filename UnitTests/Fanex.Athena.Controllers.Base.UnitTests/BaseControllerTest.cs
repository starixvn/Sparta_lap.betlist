namespace Fanex.Athena.Controllers.Base.UnitTests
{
    using System.Collections.Generic;
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
                CurrencyList = new Dictionary<string, int> { { "UUS", 1 } },
                ViewType = ClientViewType.None,
                UserId = 1,
                Language = "en-Us",
                UserName = "alex.vn",
                OnUserId = 1,
                FunctionIdList = new List<int>()
            });

            SessionService.LicenseeSession.Returns(new LicenseeSession
            {
                CurrencyName = new List<string> { "UUS" },
                LicenseeId = 100,
                LicenseeName = "Spondemo",
                LicenseeType = new List<string>()
            });

            AppSettings = Substitute.For<IAppSettings>();

            AppSettings.ClientViewType.Returns("1");
        }
    }
}