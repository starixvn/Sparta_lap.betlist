namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Core.Users;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiFunctionAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "__RequestVerificationToken";

        public ApiFunctionAuthorizationAttribute(string permissionKey)
        {
            PermissionKeys = new string[] { permissionKey };
        }

        public ApiFunctionAuthorizationAttribute(string permissionKey1, string permissionKey2)
        {
            PermissionKeys = new string[] { permissionKey1, permissionKey2 };
        }

        public string[] PermissionKeys { get; set; }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var headers = actionContext.Request.Headers;

                if (IsAjaxRequest(headers))
                {
                    if (!headers.Contains(KeyName))
                    {
                        return UnauthorizeAccessHandler(actionContext);
                    }

                    var cookie = headers
                        .GetCookies()
                        .Select(c => c[AntiForgeryConfig.CookieName])
                        .FirstOrDefault();

                    var crfsKey = headers.GetValues(KeyName).FirstOrDefault();
                    AntiForgery.Validate(cookie != null ? cookie.Value : null, crfsKey);
                }
                else
                {
                    System.Web.Helpers.AntiForgery.Validate();
                }
            }
            catch
            {
                return UnauthorizeAccessHandler(actionContext);
            }

            if (!AuthorizeRequest(actionContext))
            {
                return UnauthorizeAccessHandler(actionContext);
            }

            return continuation();
        }

        private static bool IsAjaxRequest(HttpRequestHeaders headers)
        {
            return headers.Contains("X-Requested-With") && headers.GetValues("X-Requested-With").FirstOrDefault() == "XMLHttpRequest";
        }

        private Task<HttpResponseMessage> UnauthorizeAccessHandler(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request,
                Content = new StringContent("Cheating is not good for kids.")
            };

            return FromResult(actionContext.Response);
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            var rawUrl = actionContext.Request.RequestUri;
            var authorization = new AuthorizationService();
            var authorized = true;
            if (userSession != null && licenseeSession != null)
            {
                authorized = authorization.HasPermissions(licenseeSession.LicenseeId, userSession.FunctionIdList, PermissionKeys);
            }

            return authorized;
        }

        private Task<HttpResponseMessage> FromResult(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
        }
    }
}