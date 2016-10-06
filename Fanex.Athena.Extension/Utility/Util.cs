// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Util
    {
        public static IDictionary<string, string> SupportedLanguages
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        { "en-US", "English" },
                        { "zh-TW", "繁體中文" },
                        { "zh-CN", "简体中文" },
                        { "ko-KR", "한국의" },
                        { "zh-Hans", "简体中文(新)" }
                    };
            }
        }

        /// <summary>
        /// Gets root path of site.
        /// </summary>
        /// <value>The root path url.</value>
        public static string RootPath
        {
            get
            {
                return System.Web.VirtualPathUtility.ToAbsolute("~/");
            }
        }

        /// <summary>
        /// Validate language code.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>True or false.</returns>
        public static bool IsValidLanguage(string languageCode)
        {
            var langsKey = SupportedLanguages.Keys;
            return langsKey.Contains(languageCode);
        }

        /// <summary>
        /// Return true if password contains least 2 numbers and 2 letters and have length >= 6.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The valid password or invalid password.</returns>
        [SuppressMessage(
        "StyleCop.CSharp.StylecopPlus.Rules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsPasswordValid(string password)
        {
            const string Pattern = @"^(?=.*[A-z].*[A-z])(?=.*\d.*\d).{6,}$";
            return Regex.IsMatch(password, Pattern);
        }
    }
}