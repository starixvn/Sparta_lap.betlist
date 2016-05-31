namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Helpers;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Core.Users;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ApiFunctionAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string AntiForgeryHeaderName = "X-CSRF-Token";

        public string ParentUrl { get; set; }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            try
            {
                var content = actionContext.Request.GetRouteData();

                var headers = actionContext.Request.Headers;

                var cookie = headers
                    .GetCookies()
                    .Select(c => c[AntiForgeryConfig.CookieName])
                    .FirstOrDefault();

                var crfsKey = headers.GetValues(AntiForgeryHeaderName).FirstOrDefault();

                AntiForgery.Validate(cookie != null ? cookie.Value : null, crfsKey);
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

        private Task<HttpResponseMessage> UnauthorizeAccessHandler(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Forbidden,
                RequestMessage = actionContext.ControllerContext.Request,
                Content = new StringContent("Your request was refused!!FUCKYOU:)")
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
                if (!string.IsNullOrWhiteSpace(ParentUrl))
                {
                    authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, ParentUrl);
                }

                if (authorized)
                {
                    authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, rawUrl.ToString());
                }
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