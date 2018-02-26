namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    using Attributes;
    using Extension.Utility;
    using Model.Core.DTO.Users;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;

    public class ApiFunctionAuthorizationAttributeTest
    {
        private const string KeyName = "__RequestVerificationToken";

        private IAntiForgeryService _antiForgeryService;

        private ISessionService _sessionService;

        private IAuthorizationService _authorizationService;

        [SetUp]
        public void SetUp()
        {
            _antiForgeryService = Substitute.For<IAntiForgeryService>();
            _sessionService = Substitute.For<ISessionService>();
            _authorizationService = Substitute.For<IAuthorizationService>();
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_AjaxRequestHeaderNotContainCSRF_UnauthorizeAccessResult()
        {
            // Arrange
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService);
            var actionContext = CreateActionContext(isAjax: true);

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_AjaxRequestHeaderContainCSRF_CallAntiForgeryValidate()
        {
            // Arrange
            const string CsrfValue = "abc";
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService);
            var actionContext = CreateActionContext(isAjax: true);
            actionContext.Request.Headers.Add(KeyName, CsrfValue);

            // Action
            attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            _antiForgeryService.Received(1).Validate(null, CsrfValue);
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_NotAjaxRequest_CallAntiForgeryValidate()
        {
            // Arrange
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService);
            var actionContext = CreateActionContext(isAjax: false);

            // Action
            attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            _antiForgeryService.Received(1).Validate();
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_AntiForgeryFail_ReturnUnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService);
            var actionContext = CreateActionContext();
            _antiForgeryService.When(x => x.Validate()).Throw(new Exception());

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_SessionIsNull_InvokeContinuationFunc()
        {
            // Arrange
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService);
            var actionContext = CreateActionContext();
            var continuation = Substitute.For<Func<Task<HttpResponseMessage>>>();

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), continuation);

            // Assert
            continuation.Received(1).Invoke();
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_SessionNotNull_CheckAuthorzationPermission()
        {
            // Arrange
            var permissionKeys = new[] { "abc", "def" };
            var attribute = new ApiFunctionAuthorizationAttribute(_antiForgeryService, _sessionService, _authorizationService)
            {
                PermissionKeys = permissionKeys
            };
            _sessionService.UserSession.Returns(new UserSession());
            _sessionService.LicenseeSession.Returns(new LicenseeSession());
            var actionContext = CreateActionContext();
            var continuation = Substitute.For<Func<Task<HttpResponseMessage>>>();

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), continuation);

            // Assert
            _authorizationService.Received(1).HasPermissions((IList<int>)null, permissionKeys);
        }

        private static HttpActionContext CreateActionContext(bool isAjax = false)
        {
            var controllerContext = CreateControllerContext(isAjax);

            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            return new HttpActionContext(controllerContext, descriptor);
        }

        private static HttpControllerContext CreateControllerContext(bool isAjax = false)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "test" } });
            if (isAjax)
            {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }

            return new HttpControllerContext(config, routeData, request);
        }
    }
}