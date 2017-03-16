using System;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Fanex.Athena
{
    public class CssRewriteUrlTransformStaticDomain : IItemTransform
    {
        internal static string RebaseUrlToStaticDomain(string filePath, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return url;
            }

            var uriBuilder = new StringBuilder(ConfigurationManager.AppSettings["StaticDomain"]);
            uriBuilder.Append('/');

            if (!string.IsNullOrWhiteSpace(filePath) && url[0] != '/')
            {
                uriBuilder.Append(filePath.Trim('/'));
                uriBuilder.Append('/');
            }

            uriBuilder.Append(url.TrimStart('/'));

            return uriBuilder.ToString();
        }

        internal static string ConvertUrlsToAbsolute(string filePath, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            return new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)").Replace(content, match =>
            {
                var format = match.Groups["url"].Value;
                if (format.StartsWith("data:image", StringComparison.CurrentCultureIgnoreCase))
                {
                    return format;
                }

                return "url(" + RebaseUrlToStaticDomain(filePath, format) + ")";
            });
        }

        public string Process(string includedVirtualPath, string fileContent)
        {
            if (includedVirtualPath == null)
            {
                throw new ArgumentNullException("includedVirtualPath");
            }

            var absoluteFilePath = VirtualPathUtility.ToAbsolute(includedVirtualPath);
            var filePath = VirtualPathUtility.GetDirectory(absoluteFilePath);

            return ConvertUrlsToAbsolute(filePath, fileContent);
        }
    }
}