namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using Attributes;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;
    using UnitTest.Common.Helpers;

    public class ApiBetListAuthorizationAttributeTest
    {
        private ILicenseeSettingService _licenseeSettingService;

        private ApiBetListAuthorizationAttribute _attribute;

        [SetUp]
        public void SetUp()
        {
            _licenseeSettingService = Substitute.For<ILicenseeSettingService>();
            _attribute = new ApiBetListAuthorizationAttribute();
            _attribute.LicenseeSettingService = _licenseeSettingService;
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_NotAuthorized_UnauthorizeAccessResult()
        {
            // Arrange
            var actionContext = CreateActionContext();
            _licenseeSettingService.IsAuthorizedOnBetList().Returns(false);

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_Authorized_InvokeContinuationFunc()
        {
            // Arrange
            var actionContext = CreateActionContext();
            _licenseeSettingService.IsAuthorizedOnBetList().Returns(true);
            var continuation = Substitute.For<Func<Task<HttpResponseMessage>>>();

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), continuation);

            // Assert
            continuation.Received(1).Invoke();
        }

        private static HttpActionContext CreateActionContext()
        {
            var controllerContext = ApiControllerTestHelper.CreateControllerContext();
            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            return new HttpActionContext(controllerContext, descriptor);
        }
    }
}