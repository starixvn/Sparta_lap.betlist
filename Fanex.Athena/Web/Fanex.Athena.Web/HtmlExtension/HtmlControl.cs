// -----------------------------------------------------------------------
// <copyright file="HtmlControl.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.HtmlExtension
{
    using System.Text.RegularExpressions;
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class HtmlControl
    {
        /// <summary>
        /// Remove all html tags.
        /// </summary>
        /// <param name="html">HTML (string).</param>
        /// <param name="content">Content (string).</param>
        /// <returns>String object.</returns>
        public static MvcHtmlString RemoveHtmlTag(this HtmlHelper html, string content)
        {
            var result = Regex.Replace(content, "<[^>]*>", string.Empty);
            return MvcHtmlString.Create(result);
        }
    }
}