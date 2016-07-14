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
    using Fanex.Athena.Service.Core.Users;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        #region Override

        /// <summary>
        /// On authorization.
        /// </summary>
        /// <param name="filterContext"> Parameter value.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                base.OnAuthorization(filterContext);
                var sessionService = new SessionService();
                var licenseeSession = sessionService.LicenseeSession;
                var userSession = sessionService.UserSession;
                var authorized = licenseeSession != null && userSession != null;
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

        #endregion Override
    }
}