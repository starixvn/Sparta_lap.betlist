// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;

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
                        { "zh-Hans", "简体中文(新)" }
                    };
            }
        }

        public static IDictionary<string, string> MapLanguages
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "en-US", "en" },
                    { "zh-TW", "ch" },
                    { "zh-CN", "cs" },
                    { "zh-Hans", "zhcn" },
                    { "ja-JP", "jp" },
                    { "th-TH", "th" },
                    { "vi-VN", "vn" }
                };
            }
        }

        public static IDictionary<string, string> MapIetfLanguageCodes
        {
            get
            {
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "en", "en-US" },
                    { "ch", "zh-TW" },
                    { "cs", "zh-CN" },
                    { "zhcn", "zh-Hans" },
                    { "jp", "ja-JP" },
                    { "th", "th-TH" },
                    { "vn", "vi-VN" }
                };
            }
        }

        [Obsolete]
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