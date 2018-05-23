using System.Collections.Generic;
using System.Web.Mvc;
using Fanex.Athena.Service.Core.Configuration.Interfaces;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Athena.UnitTest.Common.NUnitExtensions;
using Fanex.Athena.Web.ActionFilter;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Web.UnitTests.ActionFilter
{
    public class BetListAuthorizationAttributeTest
    {
        private readonly ControllerContext _controllerContext = MvcHelper.CreateControllerContext();

        private ILicenseeSettingService _licenseeSettingService;

        private BetListAuthorizationAttribute _attribute;

        [SetUp]
        public void Setup()
        {
            _licenseeSettingService = Substitute.For<ILicenseeSettingService>();
            _attribute = new BetListAuthorizationAttribute(_licenseeSettingService);
        }

        [Test]
        public void OnActionExecuting_IsAuthorized_FilterContextResultNull()
        {
            // Arrange
            var filterContext = CreateFilterContext(true);
            _licenseeSettingService.IsAuthorizedOnBetList().Returns(true);

            // Action
            _attribute.OnActionExecuting(filterContext);

            // Assert
            Assert.That(filterContext.Result, Is.Null);
        }

        [Test]
        public void OnActionExecuting_NotAuthorizedAjaxRequest_CorrectFilterContextResult()
        {
            // Arrange
            var filterContext = CreateFilterContext(true);
            _licenseeSettingService.IsAuthorizedOnBetList().Returns(false);
            filterContext.RequestContext.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

            // Action
            _attribute.OnActionExecuting(filterContext);
            var result = filterContext.Result as JsonResult;

            // Assert
            Assert.That(result.Data, Tobe.SameProperties(new { ErrCode = 403, ErrMsg = "Access Denied." }));
        }

        [Test]
        public void OnActionExecuting_NotAuthorized_CorrectFilterContextResult()
        {
            // Arrange
            var filterContext = CreateFilterContext(true);
            _licenseeSettingService.IsAuthorizedOnBetList().Returns(false);

            // Action
            _attribute.OnActionExecuting(filterContext);
            var result = filterContext.Result as RedirectToRouteResult;

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.RouteValues["controller"], Is.EqualTo("Utility"));
                Assert.That(result.RouteValues["action"], Is.EqualTo("AccessDenied"));
                Assert.That(result.RouteValues["message"], Is.EqualTo("AccessDenied"));
            });
        }

        private ActionExecutingContext CreateFilterContext(bool isSkipAuthorization)
        {
            var actionDescriptor = Substitute.ForPartsOf<ActionDescriptor>();

            return new ActionExecutingContext(
                _controllerContext,
                actionDescriptor,
                new Dictionary<string, object>());
        }
    }
}