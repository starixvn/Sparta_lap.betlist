namespace Fanex.Athena.Api.Base.Attributes.UnitTests
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Web.Http.Controllers;
    using Attributes;
    using DataAccess.Core.Users.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.OneAuth;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;
    using UnitTest.Common.Helpers;

    public class ApiOneAuthorizationAttributeTest
    {
        private const string TokenKey = "X-Auth-Token";

        private const string TokenValue = "abcdef";

        private ICoreLogger _logger;

        private IOneAuthenticationService _oneAuthenticationService;

        private ILicenseeDataAccess _licenseeDataAccess;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ICoreLogger>();
            _oneAuthenticationService = Substitute.For<IOneAuthenticationService>();
            _licenseeDataAccess = Substitute.For<ILicenseeDataAccess>();
        }

        [Test]
        public void ExecuteAuthorizationFilterAsync_TokenValueIsNull_UnauthorizeAccess()
        {
            // Arrange
            var attribute = new ApiOneAuthorizationAttribute();
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
            var attribute = new ApiOneAuthorizationAttribute();
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
            var attribute = new ApiOneAuthorizationAttribute();
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
            var attribute = new ApiOneAuthorizationAttribute();
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

        [Test]
        public void ExecuteAuthorizationFilterAsync_ThrowException_LogError()
        {
            // Arrange
            var exception = new Exception("123");
            var attribute = new ApiOneAuthorizationAttribute();
            var actionContext = CreateActionContext(hasAuthToken: true);

            _oneAuthenticationService.When(x => x.GetTokenInfo(TokenValue)).Throw(exception);

            // Action
            var result = attribute.ExecuteAuthorizationFilterAsync(actionContext, new CancellationToken(), null);

            // Assert
            _logger.Received(1).Error(exception);
        }

        private static HttpActionContext CreateActionContext(bool hasAuthToken = false)
        {
            var controllerContext = ApiControllerTestHelper.CreateControllerContext();
            if (hasAuthToken)
            {
                controllerContext.Request.Headers.Add(TokenKey, TokenValue);
            }

            var descriptor = Substitute.ForPartsOf<HttpActionDescriptor>();
            return new HttpActionContext(controllerContext, descriptor);
        }
    }
}