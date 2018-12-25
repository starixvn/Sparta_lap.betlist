using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Athena.Service.Core.Users.Interfaces;
using Fanex.Athena.Session.Enum;
using Fanex.Athena.Session.Interfaces;
using Fanex.Athena.Session.Model;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.UnitTest.Common.NUnitExtensions;
using Fanex.Athena.Web.ActionFilter;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class FunctionAuthorizationAttributeTest
    {
        private readonly ControllerContext _controllerContext = MvcHelper.CreateControllerContext();

        private ActionExecutingContext _filterContext;

        private ISessionStorage _sessionStorage;

        private IAuthorizationService _authorizationService;

        private FunctionAuthorizationAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _filterContext = CreateFilterContext();
            _sessionStorage = Substitute.For<ISessionStorage>();
            _authorizationService = Substitute.For<IAuthorizationService>();
            _attribute = new FunctionAuthorizationAttribute();
            _attribute.SessionStorage = _sessionStorage;
            _attribute.AuthorizationServiceInstance = _authorizationService;
        }

        [TestCase(AppMode.Parasitic)]
        [TestCase(AppMode.Self)]
        public void OnActionExecuting_IsAuthorized_FilterContextResultNull(AppMode appMode)
        {
            // Arrange
            _sessionStorage.LicenseeSession.Returns(new LicenseeSession());
            _sessionStorage.UserSession.Returns(new UserSession { ApplicationMode = appMode });
            _authorizationService.HasPermissions(Arg.Any<IEnumerable<int>>(), Arg.Any<string[]>()).Returns(true);
            _authorizationService.HasPermissions(Arg.Any<IEnumerable<string>>(), Arg.Any<string[]>()).Returns(true);

            // Action
            _attribute.OnActionExecuting(_filterContext);

            // Assert
            Assert.That(_filterContext.Result, Is.Null);
        }

        [Test]
        public void OnActionExecuting_NotAuthorizedAjaxRequest_CorrectFilterContextResult()
        {
            // Arrange
            _filterContext.RequestContext.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

            // Action
            _attribute.OnActionExecuting(_filterContext);
            var result = _filterContext.Result as JsonResult;

            // Assert
            Assert.That(result.Data, Tobe.SameProperties(new { ErrCode = 403, ErrMsg = "Access Denied." }));
        }

        [Test]
        public void OnActionExecuting_NotAuthorized_CorrectFilterContextResult()
        {
            // Action
            _attribute.OnActionExecuting(_filterContext);
            var result = _filterContext.Result as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["action"], Is.EqualTo("AccessDenied"));
                Assert.That(result.RouteValues["message"], Is.EqualTo("AccessDenied"));
            });
        }

        private ActionExecutingContext CreateFilterContext()
        {
            return new ActionExecutingContext(
                _controllerContext,
                Substitute.ForPartsOf<ActionDescriptor>(),
                new Dictionary<string, object>());
        }
    }
}