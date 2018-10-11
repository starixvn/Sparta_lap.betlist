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
    using DataAccess.Core.Users.Interfaces;
    using Extensions;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiOneAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "X-Auth-Token";

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var scope = actionContext.Request.GetDependencyScope();
            var logger = scope.Resolve<ICoreLogger>();

            try
            {
                var tokenValue = GetAuthToken(actionContext.Request);
                if (string.IsNullOrEmpty(tokenValue))
                {
                    LogInfo(logger, actionContext.Request, "Auth Token is null");
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var oneAuthenticationService = scope.Resolve<IOneAuthenticationService>();
                var token = oneAuthenticationService.GetTokenInfo(tokenValue);

                if (token == null || string.IsNullOrWhiteSpace(token.SiteName) || token.UserSEQ <= 0)
                {
                    LogInfo(logger, actionContext.Request, "TokenInfo is null");
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var licenseeDataAccess = scope.Resolve<ILicenseeDataAccess>();
                var licensee = licenseeDataAccess.GetLicenseeByName(token.SiteName);
                if (licensee == null)
                {
                    LogInfo(logger, actionContext.Request, "Licensee is null");
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
                logger.Error(exception);
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

        private static void LogInfo(ICoreLogger logger, HttpRequestMessage request, string message)
        {
            var content = new StringBuilder("ApiOneAuthorization: ");
            content.AppendLine(message);
            content.AppendLine("Url: " + request.RequestUri);
            logger.Info(content.ToString());
        }
    }
}