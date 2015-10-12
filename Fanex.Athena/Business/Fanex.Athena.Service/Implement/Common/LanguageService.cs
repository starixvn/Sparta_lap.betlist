// -----------------------------------------------------------------------
// <copyright file="LanguageService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Resources;
    using System.Threading;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;

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

        /// <summary>
        /// Set culture for a page.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        public void SetPageCulture(string languageCode)
        {
            var defaultCulture = CultureInfo.CreateSpecificCulture(_defaultLanguage);
            Thread.CurrentThread.CurrentCulture = defaultCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(languageCode);
        }

        /// <summary>
        /// Change language.
        /// </summary>
        /// <param name="languageCode">The language code.</param>
        /// <returns>True or false.</returns>
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