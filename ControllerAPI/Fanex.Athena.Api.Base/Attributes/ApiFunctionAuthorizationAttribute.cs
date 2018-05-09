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
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Users;
    using Model.Core.DataDomain;
    using Resources;
    using Service.Core.Users.Interfaces;
    using Session.Enum;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiFunctionAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string KeyName = "__RequestVerificationToken";

        private readonly IAntiForgeryService _antiForgeryService;

        private readonly ISessionService _sessionService;

        private readonly IAuthorizationService _authorizationService;

        public ApiFunctionAuthorizationAttribute()
        {
        }

        [CLSCompliant(false)]
        public ApiFunctionAuthorizationAttribute(params string[] permissionKeys)
            : this(antiForgeryService: null, sessionService: null, authorizationService: null)
        {
            PermissionKeys = permissionKeys;
        }

        public ApiFunctionAuthorizationAttribute(
            IAntiForgeryService antiForgeryService,
            ISessionService sessionService,
            IAuthorizationService authorizationService)
        {
            _antiForgeryService = antiForgeryService ?? new AntiForgeryService();
            _sessionService = sessionService ?? new SessionService();
            _authorizationService = authorizationService ?? new AuthorizationService();
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
                    _antiForgeryService.Validate(cookie != null ? cookie.Value : null, crfsKey);
                }
                else
                {
                    _antiForgeryService.Validate();
                }
            }
            catch (Exception)
            {
                return UnauthorizeAccessHandlerAsync(actionContext);
            }

            if (!AuthorizeRequest())
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

        private bool AuthorizeRequest()
        {
            var licenseeSession = _sessionService.LicenseeSession;
            var userSession = _sessionService.UserSession;
            var authorized = true;
            if (userSession != null && licenseeSession != null)
            {
                authorized = userSession.ApplicationMode == AppMode.Parasitic ?
                    _authorizationService.HasPermissions(userSession.PortalFunctionList, PermissionKeys) :
                    _authorizationService.HasPermissions(userSession.FunctionIdList, PermissionKeys);
            }

            return authorized;
        }
    }
}