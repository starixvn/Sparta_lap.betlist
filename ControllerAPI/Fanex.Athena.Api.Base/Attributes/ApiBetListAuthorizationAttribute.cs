namespace Fanex.Athena.Api.Base.Attributes
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Extensions;
    using Resources;
    using Service.Core.Configuration.Interfaces;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ApiBetListAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(
            HttpActionContext actionContext,
            CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var scope = actionContext.Request.GetDependencyScope();
            var licenseeSettingService = scope.Resolve<ILicenseeSettingService>();
            if (!licenseeSettingService.IsAuthorizedOnBetList())
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
    }
}