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
    public class CustomAuthorizeAttributeTest
    {
        private readonly ControllerContext _controllerContext = MvcHelper.CreateControllerContext();

        private ISessionStorage _sessionStorage;

        private CustomAuthorizeAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _sessionStorage = Substitute.For<ISessionStorage>();
            _attribute = new CustomAuthorizeAttribute(_sessionStorage);
        }

        [Test]
        public void OnAuthorization_SkipAuthorization_NotGetUserSession()
        {
            // Arrange
            var filterContext = CreateFilterContext(true);

            // Action
            _attribute.OnAuthorization(filterContext);

            // Assert
            var temp = _sessionStorage.DidNotReceive().UserSession;
        }

        [Test]
        public void OnAuthorization_NotSkipAuthorization_GetLicenseeSession()
        {
            // Arrange
            var filterContext = CreateFilterContext(false);

            // Action
            _attribute.OnAuthorization(filterContext);

            // Assert
            var temp = _sessionStorage.Received(1).LicenseeSession;
        }

        [Test]
        public void OnAuthorization_NotAuthorized_AssignCorrectRouteValues()
        {
            // Arrange
            var filterContext = CreateFilterContext(false);

            // Action
            _attribute.OnAuthorization(filterContext);
            var result = filterContext.Result as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["action"], Is.EqualTo("LocationRestriction"));
            });
        }

        [Test]
        public void OnAuthorization_NotAuthorized_FilterContextResultNull()
        {
            // Arrange
            var filterContext = CreateFilterContext(false);
            _sessionStorage.LicenseeSession.Returns(new LicenseeSession());
            _sessionStorage.UserSession.Returns(new UserSession());

            // Action
            _attribute.OnAuthorization(filterContext);
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