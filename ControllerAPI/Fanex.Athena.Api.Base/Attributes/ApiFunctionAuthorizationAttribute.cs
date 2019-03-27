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
    using Fanex.Athena.Service.StatelessCore.Interfaces;
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

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
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

            if (!(await AuthorizeRequest(actionContext.Request.GetDependencyScope()).ConfigureAwait(false)))
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            return continuation == null ? null : await continuation.Invoke().ConfigureAwait(false);
        }

        private static bool IsAjaxRequest(HttpRequestHeaders headers)
        {
            return headers.Contains("X-Requested-With") && headers.GetValues("X-Requested-With").FirstOrDefault() == "XMLHttpRequest";
        }

        private static HttpResponseMessage UnauthorizeAccessHandlerAsync(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request,
                Content = new StringContent(ViewResources.AccessDeniedMsg)
            };

            return actionContext.Response;
        }

        private async Task<bool> AuthorizeRequest(IDependencyScope scope)
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
                    await authorizationService.HasPermissions(userSession.FunctionIdList, _permissionKeys).ConfigureAwait(false);
            }

            return authorized;
        }
    }
}