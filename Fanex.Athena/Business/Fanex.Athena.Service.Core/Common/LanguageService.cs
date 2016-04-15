// -----------------------------------------------------------------------
// <copyright file="LanguageService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System.Globalization;
    using System.Threading;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LanguageService : ILanguageService
    {
        private readonly string _cookieName = "risk_forcast_system";
        private readonly string _defaultLanguage = "en-US";
        private readonly ISessionService _sessionService;

        public LanguageService()
        {
            _sessionService = new SessionService();
        }

        /// <summary>
        /// Get current language.
        /// </summary>
        /// <returns>The language code.</returns>
        public string GetCurrentLanguage()
        {
            var userSession = _sessionService.UserSession;
            if (userSession != null && !string.IsNullOrEmpty(userSession.Language))
            {
                return userSession.Language;
            }

            var cookiValue = CookiesHelper.GetCookies(_cookieName);
            if (string.IsNullOrEmpty(cookiValue))
            {
                return _defaultLanguage;
            }
            else
            {
                return cookiValue;
            }
        }

        public void SetPageCulture(string languageCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
        }

        public bool ChangeLanguage(string languageCode)
        {
            if (!string.IsNullOrEmpty(languageCode))
            {
                var updated = _sessionService.UpdateLanguage(languageCode);
                if (updated)
                {
                    this.SetPageCulture(languageCode);
                    CookiesHelper.SetCookies(_cookieName, languageCode);
                    return true;
                }
            }

            return false;
        }
    }
}