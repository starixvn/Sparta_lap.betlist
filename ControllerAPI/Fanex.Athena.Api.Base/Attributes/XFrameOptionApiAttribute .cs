namespace Fanex.Athena.Api.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using Model.Core.DataDomain;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class XFrameOptionApiAttribute : ActionFilterAttribute
    {
        private const string XFrameOptionHeaderName = "X-Frame-Options";
        private readonly ISessionService _sessionService;

        public XFrameOptionApiAttribute()
        {
            _sessionService = new SessionService();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            AddXFrameOption(actionExecutedContext);

            base.OnActionExecuted(actionExecutedContext);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            AddXFrameOption(actionExecutedContext);
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private void AddXFrameOption(HttpActionExecutedContext actionExecutedContext)
        {
            var user = _sessionService.UserSession;

            IEnumerable<string> headerValues;

            if (actionExecutedContext.Response.Headers.TryGetValues(XFrameOptionHeaderName, out headerValues) && headerValues.Any())
            {
                actionExecutedContext.Response.Headers.Remove(XFrameOptionHeaderName);
            }

            if (user == null || user.ApplicationMode == AppMode.Self)
            {
                actionExecutedContext.Response.Headers.Add(XFrameOptionHeaderName, "SAMEORIGIN");
            }
        }
    }
}