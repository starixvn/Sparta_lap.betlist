using System.Web.Mvc;
using Fanex.Athena.Session.Enum;
using Fanex.Athena.Session.Interfaces;
using Fanex.Athena.Session.Model;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.Web.ActionFilter;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class AuthorizeRestrictionAttributeTest
    {
        private readonly ControllerContext _controllerContext = MvcHelper.CreateControllerContext();

        private ISessionStorage _sessionStorage;

        [SetUp]
        public void Setup()
        {
            _sessionStorage = Substitute.For<ISessionStorage>();
        }

        [Test]
        public void OnAuthorization_SkipAuthorization_NotGetUserSession()
        {
            // Arrange
            var filterContext = CreateFilterContext(true);
            var attribute = new AuthorizeRestrictionAttribute(_sessionStorage, AppMode.Parasitic);

            // Action
            attribute.OnAuthorization(filterContext);

            // Assert
            var temp = _sessionStorage.DidNotReceive().UserSession;
        }

        [Test]
        public void OnAuthorization_NotSkipAuthorization_GetUserSession()
        {
            // Arrange
            var filterContext = CreateFilterContext(false);
            var attribute = new AuthorizeRestrictionAttribute(_sessionStorage, AppMode.Parasitic);

            // Action
            attribute.OnAuthorization(filterContext);

            // Assert
            var temp = _sessionStorage.Received(1).UserSession;
        }

        [Test]
        public void OnAuthorization_NotAuthorized_AssignCorrectRouteValues()
        {
            // Arrange
            var filterContext = CreateFilterContext(false);
            var attribute = new AuthorizeRestrictionAttribute(_sessionStorage, AppMode.Parasitic);

            // Action
            attribute.OnAuthorization(filterContext);
            var result = filterContext.Result as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["action"], Is.EqualTo("OneTokenRestriction"));
                Assert.That(result.RouteValues["code"], Is.EqualTo("700"));
            });
        }

        [TestCase(AppMode.Parasitic)]
        [TestCase(AppMode.Self)]
        public void OnAuthorization_NotAuthorized_FilterContextResultNull(AppMode appMode)
        {
            // Arrange
            var filterContext = CreateFilterContext(false);
            var attribute = new AuthorizeRestrictionAttribute(_sessionStorage, appMode);
            _sessionStorage.UserSession.Returns(new UserSession
            {
                ApplicationMode = appMode
            });

            // Action
            attribute.OnAuthorization(filterContext);
            var result = filterContext.Result as RedirectToRouteResult;

            // Assert
            Assert.That(result, Is.Null);
        }

        private AuthorizationContext CreateFilterContext(bool isSkipAuthorization)
        {
            var actionDescriptor = Substitute.ForPartsOf<ActionDescriptor>();
            actionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true).Returns(isSkipAuthorization);
            var filterContext = new AuthorizationContext(_controllerContext, actionDescriptor);
            return filterContext;
        }
    }
}