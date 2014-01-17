// -----------------------------------------------------------------------
// <copyright file="AjaxFunctionAuthorizationAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AjaxFunctionAuthorizationAttribute : ActionFilterAttribute
    {
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
            var authorized = authorization.AuthorizePath(licenseeSession.LicenseeId, userSession.FunctionIdList, rawUrl);

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