// -----------------------------------------------------------------------
// <copyright file="ValidateAntiForgeryTokenOnAjax.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Net;
    using System.Web.Helpers;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateAntiForgeryTokenOnAjax : AuthorizeAttribute
    {
        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            // Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                // Ajax POSTs and normal form posts have to be treated differently when it comes
                // to validating the AntiForgeryToken
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;

                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute()
                        .OnAuthorization(filterContext);
                }
            }
        }
    }
}