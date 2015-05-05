using System.Web;
using System.Web.Optimization;

namespace Fanex.Athena
{
    /// <summary>
    /// 
    /// </summary>
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        /// <summary>
        /// Processes the specified included virtual path.
        /// </summary>
        /// <param name="includedVirtualPath">The included virtual path.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}