// -----------------------------------------------------------------------
// <copyright file="HeaderController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.ViewModel;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class HeaderController : BaseController
    {
        private readonly ILanguageService _langService;
        private readonly ISessionService _sessionService;
        private readonly ILicenseeService _licenseeService;

        public HeaderController()
        {
            _sessionService = new SessionService();
            _licenseeService = new LicenseeService();
            _langService = new LanguageService();
        }

        /// <summary>
        /// Header page.
        /// </summary>
        /// <returns>The header page.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var userSession = _sessionService.UserSession;
            var viewModel = new HeaderPageViewModel
            {
                Language = userSession.Language,
                OnUserId = userSession.OnUserId,
                LicenseeList = _licenseeService.GetByUser(userSession.UserId),
                IsInternalLogin = userSession.IsInternal,
                IsDisabledRecheckIn = AppSettings.LocalTest,
                SupportedLanguageList = Util.SupportedLanguages,
                UserName = userSession.UserName,
                CurrentLicenseeId = _sessionService.LicenseeSession.LicenseeId
            };
            return View("~/Views/Header/Index.cshtml", viewModel);
        }

        /// <summary>
        /// Change language.
        /// </summary>
        /// <param name="languagesite">The language code.</param>
        /// <returns>Return "1" if changing is success.</returns>
        [HttpGet]
        public string ChangeLanguage(string languagesite)
        {
            var changed = false;
            if (!string.IsNullOrEmpty(languagesite))
            {
                var languageCode = languagesite.Trim();
                if (Util.IsValidLanguage(languageCode))
                {
                    var currentLang = _langService.GetCurrentLanguage();
                    if (!currentLang.Equals(languageCode))
                    {
                        changed = _langService.ChangeLanguage(languageCode);
                    }
                }
            }

            return changed ? "1" : "0";
        }

        /// <summary>
        /// The Sign-Page.
        /// </summary>
        /// <returns>Redirect to Sign-In page.</returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignOut()
        {
            // Kick-out
            IUserService userService = new UserService();
            var userSession = SessionService.UserSession;
            if (userSession != null)
            {
                userService.Logout(userSession.OnUserId);
            }

            Session.Clear();
            ClearCookies();
            FormsAuthentication.SignOut();
            return RedirectToAction("LocationRestriction", "Utility");
        }

        /// <summary>
        /// Clear cookies.
        /// </summary>
        private void ClearCookies()
        {
            var cookies = Request.Cookies.AllKeys;
            foreach (string cookie in cookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}