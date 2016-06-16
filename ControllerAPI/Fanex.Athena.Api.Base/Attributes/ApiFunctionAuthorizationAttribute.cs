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

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ApiFunctionAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private const string AntiForgeryHeaderName = "__RequestVerificationToken";

        public string ParentUrl { get; set; }

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
                    if (!headers.Contains(AntiForgeryHeaderName))
                    {
                        return UnauthorizeAccessHandler(actionContext);
                    }

                    var cookie = headers
                        .GetCookies()
                        .Select(c => c[AntiForgeryConfig.CookieName])
                        .FirstOrDefault();

                    var crfsKey = headers.GetValues(AntiForgeryHeaderName).FirstOrDefault();
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
                    authorized = authorization.HasPermission(licenseeSession.LicenseeId, userSession.FunctionIdList, ParentUrl);
                }

                if (authorized)
                {
                    authorized = authorization.HasPermission(licenseeSession.LicenseeId, userSession.FunctionIdList, rawUrl.ToString());
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