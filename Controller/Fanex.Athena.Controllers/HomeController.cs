// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using ONEbook.UM.Filters;
    using Model.Core.DataDomain;

    /// <summary>
    /// Home controller.
    /// </summary>
    [CheckUMFilter]
    public class HomeController : BaseController
    {
        private const string IsFirstLoadCookieKey = "isFirstLoad";

        private readonly IMoveBetService _moveBetService;

        public HomeController()
        {
            _moveBetService = new MoveBetService();
        }

        public ActionResult Index()
        {
            ViewBag.DefaultReportUrl = SessionService.UserSession.DefaultReportUrl;
            return ReAuthorize("~/Views/Home/Index.cshtml");
        }

        public ActionResult IndexIpad()
        {
            ViewBag.DefaultReportUrl = SessionService.UserSession.DefaultReportUrl;
            return ReAuthorize("~/Views/Home/IndexIpad.cshtml");
        }

        public ActionResult SwitchView(ClientViewType type = ClientViewType.Default)
        {
            SessionService.UserSession.viewType = type;
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Get number online user.
        /// </summary>
        /// <returns>The amount online users.</returns>
        [HttpGet]
        [AllowAnonymous]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        public string NumOfOnlineUser()
        {
            var result = string.Empty;
            try
            {
                ILicenseeService licenseeService = new LicenseeService();
                result = licenseeService.GetNumberOfOnlineUser();
            }
            catch (Exception exception)
            {
                result = "Error!" + exception.Message;
            }

            return result;
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

            ViewBag.NotificationContent = LoadNotificationContent();
            return View(defaultView);
        }

        private string LoadNotificationContent()
        {
            var cookie = Request.Cookies[IsFirstLoadCookieKey];
            var isFirstLoad = cookie == null ? true : cookie.Value == "1";

            if (isFirstLoad == false)
            {
                return string.Empty;
            }

            Response.SetCookie(new System.Web.HttpCookie(IsFirstLoadCookieKey, "0"));

            var moveBet = _moveBetService.GetMoveBet();
            if (!moveBet.EstimatedTime.HasValue || DateTime.Now > moveBet.EstimatedTime)
            {
                return string.Empty;
            }

            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/Notification/{0}.htm", language));
            if (!language.Equals(DefaultLanguage, System.StringComparison.OrdinalIgnoreCase) && !System.IO.File.Exists(filePath))
            {
                filePath = filePath.Replace(language, DefaultLanguage);
            }

            using (var reader = new StreamReader(filePath))
            {
                var content = reader.ReadToEnd();
                content = content.Replace("{startDate}", moveBet.AvailableWinlostDate.ToString("MM/dd/yyyy"));
                content = content.Replace("{endDate}", moveBet.EstimatedTime.Value.ToString("HH:mm:ss MM/dd/yyyy"));
                return content;
            }
        }
    }
}