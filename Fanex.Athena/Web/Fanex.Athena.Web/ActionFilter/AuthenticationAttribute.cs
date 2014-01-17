// -----------------------------------------------------------------------
// <copyright file="AuthenticationAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Service.Implement.Users;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    public class AuthenticationAttribute : ActionFilterAttribute
    {
        #region Override

        /// <summary>
        /// Check authenticated user.
        /// </summary>
        /// <param name="filterContext">Filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;

            var sessionService = new SessionService();
            var userSession = sessionService.UserSession;
            var licenseeSession = sessionService.LicenseeSession;
            if (!IsUserAuthenticated(context, userSession, licenseeSession))
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.LoginUrl);
            }

            base.OnActionExecuting(filterContext);
        }

        #endregion Override

        #region Utility

        /// <summary>
        /// Check if the current user has logged in or not.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <param name="userSession">User session.</param>
        /// <param name="licenseeSession">Licensees session.</param>
        /// <returns>True or False.</returns>
        private static bool IsUserAuthenticated(HttpContextBase context, UserSession userSession, LicenseeSession licenseeSession)
        {
            var authenticated = context.User != null
                && context.User.Identity.IsAuthenticated
                && context.Request.IsAuthenticated;

            var validSession = userSession != null
                && userSession.UserId > 0
                && licenseeSession != null
                && licenseeSession.LicenseeId > 0;

            return authenticated && validSession;
        }

        #endregion Utility
    }
}