namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Core.Users;
    using Resources;
    using Service.Core.Helpers;
    using Service.Core.Users.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiOneAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "X-Auth-Token";

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
                var tokenParam = actionContext.Request.GetQueryNameValuePairs().FirstOrDefault(x => x.Key.Equals(KeyName, StringComparison.OrdinalIgnoreCase));

                if (tokenParam.Key == null)
                {
                    return UnauthorizeAccessHandlerAsync(actionContext);
                }

                // var token = _oneAuthenticationService.GetTokenInfo(tokenParam.Value);
                var token = new Model.Core.DTO.Common.OneAuthenticationToken
                {
                    SiteName = "12bet",
                    UserSEQ = 5
                };
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
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
                return UnauthorizeAccessHandlerAsync(actionContext);
            }
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