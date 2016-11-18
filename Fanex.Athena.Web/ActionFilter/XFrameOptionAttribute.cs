namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class XFrameOptionAttribute : ActionFilterAttribute
    {
        private const string XFrameOptionAppSettingKey = "XFrameOption";
        private const string XFrameOptionHeaderName = "X-Frame-Options";
        private readonly IAppSettings _appSetting;
        private readonly ISessionService _sessionService;

        public XFrameOptionAttribute()
        {
            _sessionService = new SessionService();
            _appSetting = new AppSettings();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            AddXFrameOption(filterContext);
            base.OnActionExecuting(filterContext);
        }

        private void AddXFrameOption(ActionExecutingContext actionContext)
        {
            var user = _sessionService.UserSession;
            var value = user != null && user.ApplicationMode == AppMode.Parasitic ? _appSetting.GetValue(XFrameOptionAppSettingKey)
                : "SAMEORIGIN";

            IEnumerable<string> headerValues = actionContext.HttpContext.Response.Headers.GetValues(XFrameOptionHeaderName);

            if (headerValues != null && headerValues.Any())
            {
                actionContext.HttpContext.Response.Headers.Remove(XFrameOptionHeaderName);
            }

            actionContext.HttpContext.Response.Headers.Add(XFrameOptionHeaderName, value);
        }
    }
}