namespace Fanex.Athena
{
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
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}