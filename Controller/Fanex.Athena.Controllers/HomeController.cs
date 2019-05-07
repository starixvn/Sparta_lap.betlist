// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Extension.Utility;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;
    using Service.Core.Users.Interfaces;
    using Session.Enum;
    using Web.ActionFilter;

    [CheckUMFilter]
    public class HomeController : CultureBaseController
    {
        private readonly ILicenseeSettingService _licenseeSettingService;

        public HomeController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            ILicenseeSettingService licenseeSettingService)
        : base(languageService, sessionService, appSetting)
        {
            _licenseeSettingService = licenseeSettingService;
        }

        [AuthorizeRestriction(AppMode.Self)]
        public ActionResult Index()
        {
            AssignViewBagData();
            return ReAuthorize("~/Views/Desktop/Home/Index.cshtml");
        }

        [AuthorizeRestriction(AppMode.Self)]
        public ActionResult IndexIpad()
        {
            AssignViewBagData();
            return ReAuthorize("~/Views/Desktop/Home/IndexIpad.cshtml");
        }

        /// <summary>
        /// Checking existing user in the system.
        /// </summary>
        /// <param name="defaultView">The client device (IPAD or others).</param>
        /// <returns>Redirect to sign in page or the home page.</returns>
        private ActionResult ReAuthorize(string defaultView)
        {
            if (!HttpContext.Response.IsRequestBeingRedirected)
            {
                var user = SessionService.UserSession;
                if (user == null || user.UserId <= 0
                    || string.IsNullOrEmpty(user.UserName) || SessionService.LicenseeSession == null)
                {
                    Session.Clear();
                    return RedirectToAction("LocationRestriction", "Utility");
                }
            }

            return View(defaultView);
        }

        private void AssignViewBagData()
        {
            var returnPath = TempData["ReturnPath"];
            var userSession = SessionService.UserSession;
            ViewBag.DefaultReportUrl = returnPath == null ? userSession.DefaultReportUrl : returnPath.ToString();
            ViewBag.OnUserId = userSession.OnUserId;
            ViewBag.IsInternal = userSession.IsInternal ? "1" : "0";
            ViewBag.HasRecheckIn = AppSettings.LocalTest ? "0" : "1";
            ViewBag.IsEnabledMoveBetNotification = AppSettings.IsEnabledMoveBetNotification
                && !_licenseeSettingService.IsFakeLicensee(SessionService.LicenseeSession?.LicenseeName);
        }
    }
}