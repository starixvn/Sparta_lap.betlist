// -----------------------------------------------------------------------
// <copyright file="AuthenticationAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.ActionFilter
{
    using System.Web.Mvc;

    /// <summary>
    /// Class: AuthenticationAttribute.
    /// </summary>
    public class AuthenticationAttribute : AuthorizeAttribute
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
            }
        }

        #endregion Override
    }
}