// -----------------------------------------------------------------------
// <copyright file="CustomAuthorizeAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Session;
    using Session.Interfaces;

    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly ISessionStorage _sessionStorage;

        public CustomAuthorizeAttribute()
        {
            _sessionStorage = new SessionStorage();
        }

        public CustomAuthorizeAttribute(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
                var authorized = _sessionStorage.LicenseeSession != null && _sessionStorage.UserSession != null;
                if (!authorized)
                {
                    var routeValue = new
                    {
                        controller = "Utility",
                        action = "LocationRestriction",
                    };

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
                }
            }
        }
    }
}