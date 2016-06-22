// -----------------------------------------------------------------------
// <copyright file="CookiesHelper.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Web;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CookiesHelper
    {
        /// <summary>
        /// Get cookies.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        /// <returns>The cookie value.</returns>
        public static string GetCookies(string name)
        {
            var cookies = HttpContext.Current.Request.Cookies.Get(name);
            if (cookies != null)
            {
                return cookies.Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Set new or Update cookie.
        /// </summary>
        /// <param name="name">The cookie name.</param>
        /// <param name="value">The value.</param>
        public static void SetCookies(string name, string value)
        {
            var cookie = new HttpCookie(name, value);
            HttpContext.Current.Response.Cookies.Remove(name);
            HttpContext.Current.Response.SetCookie(cookie);
        }
    }
}