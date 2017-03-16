namespace Fanex.Athena
{
    using System.Configuration;
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// Modify URL for style sheet by using virtual directory.
    /// </summary>
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        /// <summary>
        /// Processes the specified included virtual path.
        /// </summary>
        /// <param name="includedVirtualPath">The included virtual path.</param>
        /// <param name="input">The input.</param>
        /// <returns>Url Transform Object.</returns>
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + RebaseUrlToStaticDomain(includedVirtualPath), input);
        }

        private static string RebaseUrlToStaticDomain(string includedVirtualPath)
        {
            var cdn = ConfigurationManager.AppSettings["StaticDomain"];
            return string.IsNullOrWhiteSpace(cdn)
                ? VirtualPathUtility.ToAbsolute(includedVirtualPath)
                : (cdn + includedVirtualPath.TrimStart('~'));
        }
    }
}