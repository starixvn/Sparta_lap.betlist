// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [CheckUMFilter]
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public HomeController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting)
            : base(languageService, sessionService, appSetting)
        {
        }

        [AuthorizeRestriction(AppMode.Self)]
        public ActionResult Index()
        {
            AssignViewBagData();
            return ReAuthorize("~/Views/Home/Index.cshtml");
        }

        [AuthorizeRestriction(AppMode.Self)]
        public ActionResult IndexIpad()
        {
            AssignViewBagData();
            return ReAuthorize("~/Views/Home/IndexIpad.cshtml");
        }

        public ActionResult SwitchView(ClientViewType type = ClientViewType.Default)
        {
            SessionService.UserSession.ViewType = type;
            return RedirectToAction(nameof(Index), "Home");
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
            var userSession = SessionService.UserSession;
            ViewBag.DefaultReportUrl = userSession.DefaultReportUrl;
            ViewBag.OnUserId = userSession.OnUserId;
            ViewBag.IsInternal = userSession.IsInternal ? "1" : "0";
            ViewBag.HasRecheckIn = AppSettings.LocalTest ? "0" : "1";
            ViewBag.IsEnabledMoveBetNotification = AppSettings.IsEnabledMoveBetNotification;
        }
    }
}