namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Core.Users;
    using Resources;
    using Service.Core.Users.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiOneAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "AuthToken";

        private readonly IOneAuthenticationService _oneAuthenticationService;

        private readonly ILicenseeService _licenseeService;

        public ApiOneAuthorizationAttribute()
        {
            _oneAuthenticationService = new OneAuthenticationService();
            _licenseeService = new LicenseeService();
        }

        public ApiOneAuthorizationAttribute(IOneAuthenticationService oneAuthenticationService, ILicenseeService licenseeService)
        {
            _oneAuthenticationService = oneAuthenticationService ?? new OneAuthenticationService();
            _licenseeService = licenseeService ?? new LicenseeService();
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var headers = actionContext.Request.Headers;
                if (!IsAjaxRequest(headers) || !headers.Contains(KeyName))
                {
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var tokenString = headers.GetValues(KeyName).FirstOrDefault();
                var token = _oneAuthenticationService.GetTokenInfo(tokenString);

                if (token == null || string.IsNullOrWhiteSpace(token.SiteName) || token.UserSEQ <= 0)
                {
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                var licensee = _licenseeService.GetLicenseeByName(token.SiteName);
                if (licensee == null)
                {
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                actionContext.ActionArguments["licenseeid"] = licensee.Id;
                actionContext.ActionArguments["userid"] = token.UserSEQ;

                return continuation?.Invoke();
            }
            catch (Exception)
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }
        }

        private static bool IsAjaxRequest(HttpRequestHeaders headers)
        {
            return headers.Contains("X-Requested-With") && headers.GetValues("X-Requested-With").FirstOrDefault() == "XMLHttpRequest";
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
    }
}