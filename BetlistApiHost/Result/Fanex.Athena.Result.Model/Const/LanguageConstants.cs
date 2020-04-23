namespace Fanex.Athena.Result.Model.Const
{
    using System.Collections.Generic;

    public static class LanguageConstants
    {
        private const string DefaultLanguage = "en-US";

        private static readonly Dictionary<string, string> SupportedLanguages = new Dictionary<string, string>
        {
            { "en-US", "en" },
            { "zh-CN", "cs" },
            { "zh-TW", "ch" },
            { "zh-Hans", "zhcn" },
            { "th-TH", "th" },
            { "vi-VN", "vn" },
            { "id-ID", "id" }
        };

        public static string GetLanguageCode(string language)
            => SupportedLanguages.ContainsKey(language)
            ? SupportedLanguages[language]
            : SupportedLanguages[DefaultLanguage];
    }
}