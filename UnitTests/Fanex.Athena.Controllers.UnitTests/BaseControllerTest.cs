using Fanex.Athena.Web.RouterConfig;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fanex.Athena.Controllers.UnitTests
{
    [TestFixture]
    public class BaseControllerTest
    {
        protected UrlHelper Url { get; set; }

        protected ControllerContext controllerContext { get; set; }


        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Request.ApplicationPath.Returns("/");

            controllerContext = Substitute.For<ControllerContext>();
            controllerContext.HttpContext = httpContext;

            var requestContext = new RequestContext(httpContext, new RouteData());

            Url = new UrlHelper(requestContext, routes);
        }
    }
}
