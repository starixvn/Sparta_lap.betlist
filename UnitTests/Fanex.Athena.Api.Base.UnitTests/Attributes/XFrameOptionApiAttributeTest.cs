namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Autofac;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;
    using Session.Enum;
    using Session.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class XFrameOptionApiAttributeTest
    {
        private const string XFrameOptionHeaderName = "X-Frame-Options";

        private ISessionStorage _sessionStorage;

        private HttpActionExecutedContext _actionExecutedContext;

        [SetUp]
        public void SetUp()
        {
            _sessionStorage = Substitute.For<ISessionStorage>();
            _actionExecutedContext = CreateActionExecutedContext();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void OnActionExecuted_UserSessionIsNull_XFrameOptionToSameOrigin(bool hasXframeOptionHeader)
        {
            // Arrange
            var attribute = new XFrameOptionApiAttribute();
            if (hasXframeOptionHeader)
            {
                _actionExecutedContext.Response.Headers.Add(XFrameOptionHeaderName, "any value");
            }

            // Action
            attribute.OnActionExecuted(_actionExecutedContext);

            // Assert
            Assert.That(_actionExecutedContext.Response.Headers.GetValues(XFrameOptionHeaderName).First(), Is.EqualTo("SAMEORIGIN"));
        }

        [Test]
        public void OnActionExecuted_UserAppModeIsParasitic_NotAddXFrameOption()
        {
            // Arrange
            var attribute = new XFrameOptionApiAttribute();
            _sessionStorage.UserSession.Returns(new UserSession
            {
                ApplicationMode = AppMode.Parasitic
            });

            // Action
            attribute.OnActionExecuted(_actionExecutedContext);

            // Assert
            Assert.That(_actionExecutedContext.Response.Headers.Contains(XFrameOptionHeaderName), Is.False);
        }

        private HttpActionExecutedContext CreateActionExecutedContext()
        {
            var controllerContext = ApiControllerTestHelper.CreateControllerContext();

            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            var actionContext = new HttpActionContext(controllerContext, descriptor);
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_sessionStorage);
            WebApiHelper.SetupResolver(builder, actionContext);
            var actionExecutedContext = new HttpActionExecutedContext(actionContext, new Exception())
            {
                Response = new HttpResponseMessage()
            };
            
            return actionExecutedContext;
        }
    }
}