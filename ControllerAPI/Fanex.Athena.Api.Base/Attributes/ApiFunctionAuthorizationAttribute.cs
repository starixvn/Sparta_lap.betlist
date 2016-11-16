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
    using Model.Core.DataDomain;
    using Resources;

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

        public ApiFunctionAuthorizationAttribute(string permissionKey1, string permissionKey2, string permissionKey3)
        {
            PermissionKeys = new string[] { permissionKey1, permissionKey2, permissionKey3 };
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
                        return UnauthorizeAccessHandlerAsync(actionContext);
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
                    AntiForgery.Validate();
                }
            }
            catch (Exception)
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            if (!AuthorizeRequest(actionContext))
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            return continuation?.Invoke();
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

            return FromResultAsync(actionContext.Response);
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
                authorized =
              userSession.ApplicationMode == AppMode.Parasitic ?

                    authorization.HasPermissions(userSession.PortalFunctionList, PermissionKeys) :

                    authorization.HasPermissions(userSession.FunctionIdList, PermissionKeys);
            }

            return authorized;
        }

        private static Task<HttpResponseMessage> FromResultAsync(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
        }
    }
}