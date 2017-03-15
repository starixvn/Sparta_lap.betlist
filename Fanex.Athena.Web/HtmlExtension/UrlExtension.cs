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

    public static class UrlExtension
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Extension method")]
        public static string ContentLink(this UrlHelper helper, string contentPath)
        {
            var appPath = ConfigurationManager.AppSettings["Environment"] != "DEV" ?
                 ConfigurationManager.AppSettings["StaticDomain"] :
                 HttpContext.Current.Request.ApplicationPath;
            var baseUrl = appPath != null && appPath.Length > 1 ? appPath : string.Empty;

            return baseUrl + contentPath + "?" + ConfigurationManager.AppSettings["JsVersion"];
        }

        public static string ContentLinkByEnv(this UrlHelper helper, string contentPath)
        {
            var isCssOrJsPath = contentPath.IndexOf("/js/build/", System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                                contentPath.IndexOf("/css/build/", System.StringComparison.OrdinalIgnoreCase) >= 0;
            if (isCssOrJsPath)
            {
                var environmentFolder = ConfigurationManager.AppSettings["LocalTest"] == "1" ? "dev" : "pro";
                contentPath = Regex.Replace(contentPath, "/build/", $"/build/{environmentFolder}/");
            }

            return ContentLink(helper, contentPath);
        }
    }
}