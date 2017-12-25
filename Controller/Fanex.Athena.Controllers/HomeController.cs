// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Extension.Utility.Interfaces;
    using Model.Core.DataDomain;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;
    using Service.Core.Helpers;

    [CheckUMFilter]
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public HomeController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
               IClientViewHelper clientViewHelper
            )
            : base(languageService, sessionService, appSetting, clientViewHelper)
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

        public ActionResult SwitchView(ClientViewType type = ClientViewType.Default, string returnPath = null)
        {
            SessionService.UserSession.ViewType = type;
            TempData["ReturnPath"] = returnPath;
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
            var returnPath = TempData["ReturnPath"];
            var userSession = SessionService.UserSession;
            ViewBag.DefaultReportUrl = returnPath == null ? userSession.DefaultReportUrl : returnPath.ToString();
            ViewBag.OnUserId = userSession.OnUserId;
            ViewBag.IsInternal = userSession.IsInternal ? "1" : "0";
            ViewBag.HasRecheckIn = AppSettings.LocalTest ? "0" : "1";
            ViewBag.IsEnabledMoveBetNotification = AppSettings.IsEnabledMoveBetNotification;
        }
    }
}