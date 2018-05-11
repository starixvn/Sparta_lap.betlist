namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Fanex.Athena.Service.Core.Users;
    using Resources;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiBetListAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly ISessionService _sessionService;

        private readonly IFakeLicenseeSettingService _fakeLicenseeSettingService;

        public ApiBetListAuthorizationAttribute() : this(null, null)
        {
        }

        public ApiBetListAuthorizationAttribute(
            ISessionService sessionService,
            IFakeLicenseeSettingService fakeLicenseeSettingService)
        {
            _sessionService = sessionService ?? new SessionService();
            _fakeLicenseeSettingService = fakeLicenseeSettingService ?? new FakeLicenseeSettingService();
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
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
            var authorized = _sessionService.LicenseeSession != null;
            if (authorized)
            {
                var setting = _fakeLicenseeSettingService.GetSetting();
                if (setting != null)
                {
                    authorized = !setting.DisableBetList;
                }
            }

            return authorized;
        }
    }
}