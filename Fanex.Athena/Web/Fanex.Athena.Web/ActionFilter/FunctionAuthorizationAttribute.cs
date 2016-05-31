// -----------------------------------------------------------------------
// <copyright file="FunctionAuthorizationAttribute.cs" company="Nexcel">
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
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class FunctionAuthorizationAttribute : ActionFilterAttribute
    {
        public string ParentUrl { get; set; }

        #region Override

        /// <summary>
        /// Check authorized function.
        /// </summary>
        /// <param name="filterContext">Filter Context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var sessionService = new SessionService();
            var licenseeSession = sessionService.LicenseeSession;
            var userSession = sessionService.UserSession;
            var rawUrl = context.Request.RawUrl;
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
                    authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, rawUrl);
                }
            }

            if (!authorized)
            {
                var routeValue = new
                 {
                     controller = "Utility",
                     action = "AccessDenied",
                     message = "AccessDenied"
                 };

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion Override
    }
}