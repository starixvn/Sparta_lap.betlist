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
    using System.Web.Http.Dependencies;
    using System.Web.Http.Filters;
    using Extension.Utility;
    using Extensions;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Session.Enum;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiFunctionAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "__RequestVerificationToken";

        private readonly string[] _permissionKeys;

        public ApiFunctionAuthorizationAttribute()
        {
        }

        public ApiFunctionAuthorizationAttribute(params string[] permissionKeys)
        {
            _permissionKeys = permissionKeys;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var headers = actionContext.Request.Headers;
                var scope = actionContext.Request.GetDependencyScope();
                var antiForgeryService = scope.Resolve<IAntiForgeryService>();
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

                    antiForgeryService.Validate(cookie != null ? cookie.Value : null, crfsKey);
                }
                else
                {
                    antiForgeryService.Validate();
                }
            }
            catch
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            if (!AuthorizeRequest(actionContext.Request.GetDependencyScope()))
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            return continuation?.Invoke();
        }

        private static Task<HttpResponseMessage> FromResultAsync(HttpResponseMessage result)
        {
            var source = new TaskCompletionSource<HttpResponseMessage>();
            source.SetResult(result);
            return source.Task;
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

        private bool AuthorizeRequest(IDependencyScope scope)
        {
            var sessionService = scope.Resolve<ISessionService>();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            var authorized = userSession != null && licenseeSession != null;
            if (authorized)
            {
                var authorizationService = scope.Resolve<IAuthorizationService>();
                authorized = userSession.ApplicationMode == AppMode.Parasitic ?
                    authorizationService.HasPermissions(userSession.PortalFunctionList, _permissionKeys) :
                    authorizationService.HasPermissions(userSession.FunctionIdList, _permissionKeys);
            }

            return authorized;
        }
    }
}