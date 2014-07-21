// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Gets languages.
        /// </summary>
        /// <value>A dictionary contains language codes and language names.</value>
        public static Dictionary<string, string> SupportedLanguages
        {
            get
            {
                return new Dictionary<string, string>
                    {
                        { "en-US", "English" },
                        { "zh-TW", "繁體中文" },
                        { "zh-CN", "简体中文" },
                        { "ko-KR", "한국의" }
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
        /// Validate bet type by product.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="bettype">The bet type.</param>
        /// <returns>True or false.</returns>
        [SuppressMessage(
        "StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public static bool ValidateBettpe(string productCode, int bettype)
        {
            const int SBMinBetType = 30;
            const int SBMaxBetType = 999;
            const int SBMinOfMaxBettype = 120;
            const int SBMinSportingSolutionBetType = 1301;
            const int SBMaxSportingSolutionBetType = 1334;
            const int NGMinBetType = 81;
            const int NGMaxBetType = 119;
            const int RBMinBetType = 31;
            const int RBMaxBetType = 33;
            const int VSMinBetType = 1201;
            const int VSMaxBetType = 1299;
            const int LCMinBetType = 1101;
            const int LCMaxBetType = 1199;
            const int CSMinBetType = 71;
            const int CSMaxBetType = 71;
            const int BGMinBetType = 73;
            const int BGMaxBetType = 73;
            const int KenoMinBetType = 1501;
            const int KenoMaxBetType = 1599;

            if (string.IsNullOrEmpty(productCode))
            {
                return false;
            }

            var code = productCode.ToUpper();
            switch (code)
            {
                case "SB":
                    return bettype <= SBMinBetType || (bettype >= SBMinOfMaxBettype && bettype <= SBMaxBetType) ||
                        (bettype >= SBMinSportingSolutionBetType && bettype <= SBMaxSportingSolutionBetType);
                case "BA":
                    return bettype <= SBMinBetType || (bettype >= SBMinOfMaxBettype && bettype <= SBMaxBetType);
                case "RB":
                    return bettype >= RBMinBetType && bettype <= RBMaxBetType;
                case "NG":
                    return bettype >= NGMinBetType && bettype <= NGMaxBetType;
                case "VS":
                    return bettype >= VSMinBetType && bettype <= VSMaxBetType;
                case "LC":
                    return bettype >= LCMinBetType && bettype <= LCMaxBetType;
                case "BG":
                    return bettype >= BGMinBetType && bettype <= BGMaxBetType;
                case "CS":
                    return bettype >= CSMinBetType && bettype <= CSMaxBetType;
                case "KN":
                    return bettype >= KenoMinBetType && bettype <= KenoMaxBetType;
            }

            return false;
        }

        /// <summary>
        /// Return true if password contains least 2 numbers and 2 letters and have length >= 6.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The valid password or invalid password.</returns>
        [SuppressMessage(
        "StyleCop.CSharp.Nexcel.NexcelCustomRules",
        "SP2101:MethodMustNotContainMoreLinesThan",
        Justification = "Reviewed. Suppression is OK here.")]
        public static bool CheckValidPassword(string password)
        {
            password = password.ToLower();
            if (password.Length < 6)
            {
                return false;
            }

            var isValid = false;
            var j3 = -1;
            for (var i = 0; i < 10; i++)
            {
                var j1 = password.IndexOf(i.ToString(), System.StringComparison.OrdinalIgnoreCase);
                var j2 = password.LastIndexOf(i.ToString(), System.StringComparison.OrdinalIgnoreCase);
                if (j1 != j2)
                {
                    isValid = true;
                    break;
                }

                if (j3 == -1)
                {
                    j3 = j1;
                    continue;
                }

                if (j3 == -1 || j3 == j1)
                {
                    continue;
                }

                isValid = true;
                break;
            }

            if (isValid)
            {
                const string AtoZ = "abcdefghijklmnopqrstuvwxyz";
                var count = 0;
                for (var i = 0; i < password.Length; i++)
                {
                    if (AtoZ.Contains(password.Substring(i, 1)))
                    {
                        ++count;
                    }

                    if (count != 2)
                    {
                        continue;
                    }

                    break;
                }

                if (count < 2)
                {
                    isValid = false;
                }
            }

            return isValid;
        }
    }
}