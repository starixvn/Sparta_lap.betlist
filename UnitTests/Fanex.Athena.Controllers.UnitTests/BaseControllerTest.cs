using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Core.DataDomain;
using Fanex.Athena.Service.Core.Common.Interfaces;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Athena.Web.RouterConfig;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Controllers.UnitTests
{
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
            SessionService.UserSession.Returns(new Model.Core.DTO.Users.UserSession
            {
                ViewType = ClientViewType.None,
                UserId = 1,
                Language = "en-Us",
                UserName = "alex.vn",
                OnUserId = 1,
                FunctionIdList = new List<int>()
            });

            AppSettings = Substitute.For<IAppSettings>();

            AppSettings.ClientViewType.Returns("1");
        }
    }
}