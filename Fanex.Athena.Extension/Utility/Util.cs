// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Extension.Utility
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Get languages.
        /// </summary>
        /// <returns>The languages.</returns>
        public static Dictionary<string, string> GetSupportedLanguage()
        {
            return new Dictionary<string, string>
            {
                { "en-US", "English" },
                { "zh-TW", "繁體中文" },
                { "zh-CN", "简体中文" },
                { "ko-KR", "한국의" }
            };
        }

        /// <summary>
        /// Validate language code.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>True or false.</returns>
        public static bool IsValidLanguage(string languageCode)
        {
            var langsKey = GetSupportedLanguage().Keys;
            return langsKey.Contains(languageCode);
        }

        /// <summary>
        /// Get root path of site.
        /// </summary>
        /// <returns>The root path url.</returns>
        public static string GetRootPath()
        {
            return System.Web.VirtualPathUtility.ToAbsolute("~/");
        }

        /// <summary>
        /// Validate bet type by product.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="bettype">The bet type.</param>
        /// <returns>True or false.</returns>
        public static bool ValidateBettpe(string productCode, int bettype)
        {
            const int SBMinBetType = 30;
            const int SBMaxBetType = 999;
            const int SBMinOfMaxBettype = 120;
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
            if (string.IsNullOrEmpty(productCode))
            {
                return false;
            }

            var code = productCode.ToUpper();
            switch (code)
            {
                case "SB":
                    return bettype <= SBMinBetType || (bettype >= SBMinOfMaxBettype && bettype <= SBMaxBetType);
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
            }

            return false;
        }
    }
}