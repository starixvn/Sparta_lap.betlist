// -----------------------------------------------------------------------
// <copyright file="UrlExtension.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Web.HtmlExtension
{
    using System.Configuration;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Mvc;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Extension method")]
        public static string ContentLink(this UrlHelper helper, string contentPath)
        {
            var appPath = HttpContext.Current.Request.ApplicationPath;
            var baseUrl = appPath != null && appPath.Length > 1 ? appPath : string.Empty;

            return baseUrl + contentPath + "?" + ConfigurationManager.AppSettings["JsVersion"];
        }

        public static string ContentLinkByEnv(this UrlHelper helper, string contentPath)
        {
            if (contentPath.IndexOf("/js/build/", System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                var environmentFolder = ConfigurationManager.AppSettings["LocalTest"] == "1" ? "dev" : "pro";
                contentPath = Regex.Replace(contentPath, "/js/build/", $"/js/build/{environmentFolder}/");
            }

            return ContentLink(helper, contentPath);
        }
    }
}