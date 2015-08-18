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
        /// Map function name with resource language.
        /// </summary>
        /// <param name="list">The functions.</param>
        /// <returns>The mapped functions.</returns>
        public IList<Function> MapWithLanguage(IList<Function> list)
        {
            if (list == null)
            {
                return null;
            }

            var currentResource = Type.GetType("Fanex.Athena.Models.ViewModel.Menu, Fanex.Athena.Models");
            ResourceManager rm = new ResourceManager(currentResource);
            var newList = new List<Function>();
            foreach (var func in list)
            {
                newList.Add(new Function
                {
                    IsActive = func.IsActive,
                    Description = func.Description,
                    DisplayOrder = func.DisplayOrder,
                    FunctionId = func.FunctionId,
                    IsMenu = func.IsMenu,
                    IsNew = func.IsNew,
                    Name = rm.GetString("Menu_" + func.FunctionId),
                    ParentId = func.ParentId,
                    Url = func.Url
                });
            }

            return newList;
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

            CultureInfo def = System.Globalization.CultureInfo.CreateSpecificCulture(_defaultLanguage);

            Thread.CurrentThread.CurrentCulture = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(languageCode);

            Thread.CurrentThread.CurrentCulture.DateTimeFormat = def.DateTimeFormat;
            Thread.CurrentThread.CurrentCulture.NumberFormat = def.NumberFormat;

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