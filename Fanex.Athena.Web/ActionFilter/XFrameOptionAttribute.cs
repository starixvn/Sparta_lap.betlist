namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;
    using Session.Enum;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class XFrameOptionAttribute : ActionFilterAttribute
    {
        private const string XFrameOptionHeaderName = "X-Frame-Options";

        public ISessionService SessionService { get; set; }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AddXFrameOption(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void AddXFrameOption(ActionExecutingContext actionContext)
        {
            var user = SessionService.UserSession;

            IEnumerable<string> headerValues = actionContext.HttpContext.Response.Headers.GetValues(XFrameOptionHeaderName);

            if (headerValues != null && headerValues.Any())
            {
                actionContext.HttpContext.Response.Headers.Remove(XFrameOptionHeaderName);
            }

            if (user == null || user.ApplicationMode == AppMode.Self)
            {
                actionContext.HttpContext.Response.Headers.Add(XFrameOptionHeaderName, "SAMEORIGIN");
            }
        }
    }
}