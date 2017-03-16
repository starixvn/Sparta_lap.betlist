namespace Fanex.Athena
{
    using System;
    using System.Configuration;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// Modify URL for style sheet by using virtual directory.
    /// </summary>
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            var cdnDomain = ConfigurationManager.AppSettings["StaticDomain"];

            return string.IsNullOrWhiteSpace(cdnDomain)
                ? new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input)
                : ConvertUrlsCdnDomain(cdnDomain, VirtualPathUtility.GetDirectory(includedVirtualPath.TrimStart('~')), input);
        }

        private static string ConvertUrlsCdnDomain(string cdnDomain, string relativePath, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            var matchEvaluator = new MatchEvaluator(match =>
            {
                var format = match.Groups["url"].Value;
                return format.StartsWith("data:image", StringComparison.CurrentCultureIgnoreCase)
                    ? format
                    : "url(" + RebaseUrlToCdnDomain(cdnDomain, relativePath, format) + ")";
            });

            return new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)").Replace(content, matchEvaluator);
        }

        private static string RebaseUrlToCdnDomain(string cdnDomain, string relativePath, string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url[0] == '/')
            {
                return url;
            }

            return cdnDomain + "/" + VirtualPathUtility.Combine(relativePath, url).TrimStart('/');
        }
    }
}