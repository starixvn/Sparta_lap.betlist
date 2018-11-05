namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using Attributes;
    using Autofac;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class ApiBetListAuthorizationAttributeTest
    {
        private const string LicenseeName = "sponXXX";

        private ILicenseeSettingService _licenseeSettingService;

        private ISessionService _sessionService;

        private ApiBetListAuthorizationAttribute _attribute;

        private HttpActionContext _actionContext;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _licenseeSettingService = Substitute.For<ILicenseeSettingService>();
            _sessionService = Substitute.For<ISessionService>();
            _sessionService.LicenseeSession.Returns(new LicenseeSession
            {
                LicenseeName = LicenseeName
            });

            _attribute = new ApiBetListAuthorizationAttribute();
            _actionContext = CreateActionContext();
            var builder = new ContainerBuilder();
            builder.RegisterInstance(_licenseeSettingService);
            builder.RegisterInstance(_sessionService);
            WebApiHelper.SetupResolver(builder, _actionContext);
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_NotAuthorized_UnauthorizeAccessResult()
        {
            // Arrange
            _licenseeSettingService.IsAuthorizedOnBetList(Arg.Any<string>()).Returns(false);

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(_actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_Authorized_InvokeContinuationFunc()
        {
            // Arrange
            _licenseeSettingService.IsAuthorizedOnBetList(LicenseeName).Returns(true);
            var continuation = Substitute.For<Func<Task<HttpResponseMessage>>>();

            // Action
            var result = _attribute.ExecuteAuthorizationFilterAsync(_actionContext, new CancellationToken(), continuation);

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