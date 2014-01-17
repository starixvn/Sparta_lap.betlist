// -----------------------------------------------------------------------
// <copyright file="UrlExtension.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.HtmlExtension
{
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;

    /// <summary>
    /// Url Extension.
    /// </summary>
    public static class UrlExtension
    {
        /// <summary>
        /// Get full url.
        /// </summary>
        /// <param name="helper">The url helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The full url.</returns>
        public static string ContentLink(this UrlHelper helper, string contentPath)
        {
            var appPath = HttpContext.Current.Request.ApplicationPath;
            var baseUrl = appPath != null && appPath.Length > 1 ? appPath : string.Empty;

            return baseUrl + contentPath + "?" + AppSettings.JsVersion;
        }
    }
}