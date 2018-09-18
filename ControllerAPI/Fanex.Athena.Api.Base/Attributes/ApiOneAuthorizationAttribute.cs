namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using DataAccess.Core.Users;
    using DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Logging;
    using Resources;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiOneAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "X-Auth-Token";

        private readonly ICoreLogger _logger;

        private readonly IOneAuthenticationService _oneAuthenticationService;

        private readonly ILicenseeDataAccess _licenseeDataAccess;

        public ApiOneAuthorizationAttribute()
        {
            _logger = new CoreLogger();
            _oneAuthenticationService = new OneAuthenticationService();
            _licenseeDataAccess = new LicenseeDataAccess();
        }

        [CLSCompliant(false)]
        public ApiOneAuthorizationAttribute(ICoreLogger logger, IOneAuthenticationService oneAuthenticationService, ILicenseeDataAccess licenseeDataAccess)
        {
            _logger = logger ?? new CoreLogger();
            _oneAuthenticationService = oneAuthenticationService ?? new OneAuthenticationService();
            _licenseeDataAccess = licenseeDataAccess ?? new LicenseeDataAccess();
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var tokenValue = GetAuthToken(actionContext.Request);
                if (string.IsNullOrEmpty(tokenValue))
                {
                    LogInfo(actionContext.Request, "Auth Token is null");
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var token = _oneAuthenticationService.GetTokenInfo(tokenValue);

                if (token == null || string.IsNullOrWhiteSpace(token.SiteName) || token.UserSEQ <= 0)
                {
                    LogInfo(actionContext.Request, "TokenInfo is null");
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var licensee = _licenseeDataAccess.GetLicenseeByName(token.SiteName);
                if (licensee == null)
                {
                    LogInfo(actionContext.Request, "Licensee is null");
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                actionContext.ActionArguments["licenseeid"] = licensee.Id;
                actionContext.ActionArguments["userid"] = token.UserSEQ;
                actionContext.ActionArguments["currencyIDList"] = string.Join(",", token.Currency.Select(i => i.CurrencyId).ToArray());
                actionContext.ActionArguments["username"] = token.UserID;
                actionContext.ActionArguments["sitename"] = token.SiteName;
                actionContext.ActionArguments["language"] = token.Language;

                return continuation?.Invoke();
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
                return UnauthorizeAccessHandlerAsync(actionContext);
            }
        }

        private static string GetAuthToken(HttpRequestMessage request)
        {
            IEnumerable<string> headerValues = null;
            if (request.Headers.TryGetValues(KeyName, out headerValues))
            {
                return headerValues.FirstOrDefault();
            }

            return request.GetQueryNameValuePairs().FirstOrDefault(x => x.Key.Equals(KeyName, StringComparison.OrdinalIgnoreCase)).Value;
        }

        private static Task<HttpResponseMessage> UnauthorizeAccessHandlerAsync(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request,
                Content = new StringContent(ViewResources.AccessDeniedMsg)
            };

            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(actionContext.Response);
            return source.Task;
        }

        private void LogInfo(HttpRequestMessage request, string message)
        {
            var content = new StringBuilder("ApiOneAuthorization: ");
            content.AppendLine(message);
            content.AppendLine("Url: " + request.RequestUri);
            _logger.Info(content.ToString());
        }
    }
}