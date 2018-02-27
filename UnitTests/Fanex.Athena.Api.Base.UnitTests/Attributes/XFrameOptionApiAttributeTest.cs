namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Http.Routing;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;

    public class XFrameOptionApiAttributeTest
    {
        private const string XFrameOptionHeaderName = "X-Frame-Options";

        private ISessionService _sessionService;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnActionExecuted_UserSessionIsNull_XFrameOptionToSameOrigin(bool hasXframeOptionHeader)
        {
            // Arrange
            var attribute = new XFrameOptionApiAttribute(_sessionService);
            var actionExecutedContext = CreateActionExecutedContext();
            if (hasXframeOptionHeader)
            {
                actionExecutedContext.Response.Headers.Add(XFrameOptionHeaderName, "any value");
            }

            // Action
            attribute.OnActionExecuted(actionExecutedContext);

            // Assert
            Assert.That(actionExecutedContext.Response.Headers.GetValues(XFrameOptionHeaderName).First(), Is.EqualTo("SAMEORIGIN"));
        }

        [Test]
        public void OnActionExecuted_UserAppModeIsParasitic_NotAddXFrameOption()
        {
            // Arrange
            var attribute = new XFrameOptionApiAttribute(_sessionService);
            var actionExecutedContext = CreateActionExecutedContext();
            _sessionService.UserSession.Returns(new UserSession
            {
                ApplicationMode = AppMode.Parasitic
            });

            // Action
            attribute.OnActionExecuted(actionExecutedContext);

            // Assert
            Assert.That(actionExecutedContext.Response.Headers.Contains(XFrameOptionHeaderName), Is.False);
        }

        private static HttpActionExecutedContext CreateActionExecutedContext()
        {
            var controllerContext = CreateControllerContext();

            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            var actionContext = new HttpActionContext(controllerContext, descriptor);
            return new HttpActionExecutedContext(actionContext, new Exception())
            {
                Response = new HttpResponseMessage()
            };
        }

        private static HttpControllerContext CreateControllerContext()
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "test" } });

            return new HttpControllerContext(config, routeData, request);
        }
    }
}