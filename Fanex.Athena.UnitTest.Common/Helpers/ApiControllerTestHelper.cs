using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace Fanex.Athena.UnitTest.Common.Helpers
{
    public static class ApiControllerTestHelper
    {
        public static HttpControllerContext CreateControllerContext()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri("http://localhost/api/test"));
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "test" } });
            return new HttpControllerContext(config, routeData, request);
        }
    }
}