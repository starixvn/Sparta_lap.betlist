// --------------------------------------------------------------------------
// <copyright file="FunctionAuthorizationAttribute.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    ///  Only use for left menu URL.
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

            var ajaxRequest = context.Request["X-Requested-With"] == "XMLHttpRequest";
            ajaxRequest = ajaxRequest || ((context.Request.Headers != null)
                            && (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest"));

            if (!ajaxRequest)
            {
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
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion Override
    }
}