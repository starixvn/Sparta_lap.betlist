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
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class ApiBetListAuthorizationAttributeTest
    {
        private ISessionService _sessionService;

        private IFakeLicenseeSettingService _fakeLicenseeSettingService;

        private ApiBetListAuthorizationAttribute _attribute;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _fakeLicenseeSettingService = Substitute.For<IFakeLicenseeSettingService>();
            _attribute = new ApiBetListAuthorizationAttribute(_sessionService, _fakeLicenseeSettingService);
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_NotAuthorizedRequest_UnauthorizeAccessResult()
        {
            // Arrange
            var actionContext = CreateActionContext();

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_FakeLicenseeDisableBetList_UnauthorizeAccessResult()
        {
            // Arrange
            var actionContext = CreateActionContext();
            _sessionService.LicenseeSession.Returns(new LicenseeSession());
            _fakeLicenseeSettingService.GetSetting().Returns(new FakeLicenseeSetting
            {
                DisableBetList = true
            });

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_NotFakeLicensee_InvokeContinuationFunc()
        {
            // Arrange
            var actionContext = CreateActionContext();
            _sessionService.LicenseeSession.Returns(new LicenseeSession());
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