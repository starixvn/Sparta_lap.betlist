namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;
    using Attributes;
    using DataAccess.Core.Users.Interfaces;
    using Logging;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;

    public class ApiOneAuthorizationAttributeTest
    {
        private const string TokenKey = "X-Auth-Token";

        private const string TokenValue = "abcdef";

        private ILogger _logger;

        private IOneAuthenticationService _oneAuthenticationService;

        private ILicenseeDataAccess _licenseeDataAccess;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_TokenValueIsNull_UnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiOneAuthorizationAttribute(_logger, _oneAuthenticationService, _licenseeDataAccess);
            var actionContext = CreateActionContext();

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_AuthTokenNull_UnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiOneAuthorizationAttribute(_logger, _oneAuthenticationService, _licenseeDataAccess);
            var actionContext = CreateActionContext(hasAuthToken: true);

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.Multiple(() =>
            {
                _oneAuthenticationService.Received(1).GetTokenInfo(TokenValue);
                Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_LicenseeIsNull_UnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiOneAuthorizationAttribute(_logger, _oneAuthenticationService, _licenseeDataAccess);
            var actionContext = CreateActionContext(hasAuthToken: true);
            var token = new OneAuthToken
            {
                UserSEQ = 1,
                SiteName = "spondemo"
            };
            _oneAuthenticationService.GetTokenInfo(TokenValue).Returns(token);

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.Multiple(() =>
            {
                _licenseeDataAccess.Received(1).GetLicenseeByName(token.SiteName);
                Assert.That(result.Result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_LicenseeNotNull_UnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiOneAuthorizationAttribute(_logger, _oneAuthenticationService, _licenseeDataAccess);
            var actionContext = CreateActionContext(hasAuthToken: true);
            var token = new OneAuthToken
            {
                UserSEQ = 1,
                SiteName = "spondemo",
                Currency = new[] { new OneAuthCurrency { CurrencyId = 123 } },
                UserID = "badman"
            };
            _oneAuthenticationService.GetTokenInfo(TokenValue).Returns(token);
            var licensee = new Licensee { Id = 999 };
            _licenseeDataAccess.GetLicenseeByName(token.SiteName).Returns(licensee);

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actionContext.ActionArguments["licenseeid"], Is.EqualTo(licensee.Id));
                Assert.That(actionContext.ActionArguments["userid"], Is.EqualTo(token.UserSEQ));
                Assert.That(actionContext.ActionArguments["currencyIDList"], Is.EqualTo(token.Currency[0].CurrencyId.ToString()));
                Assert.That(actionContext.ActionArguments["username"], Is.EqualTo(token.UserID));
                Assert.That(actionContext.ActionArguments["sitename"], Is.EqualTo(token.SiteName));
            });
        }

        private static HttpActionContext CreateActionContext(bool hasAuthToken = false)
        {
            var controllerContext = CreateControllerContext(hasAuthToken);

            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            return new HttpActionContext(controllerContext, descriptor);
        }

        private static HttpControllerContext CreateControllerContext(bool hasAuthToken = false)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
            var route = config.Routes.MapHttpRoute("default", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "test" } });
            if (hasAuthToken)
            {
                request.Headers.Add(TokenKey, TokenValue);
            }

            return new HttpControllerContext(config, routeData, request);
        }
    }
}