// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.IO;
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : BaseController
    {
        private readonly IMoveBetService _moveBetService;

        public HomeController()
        {
            _moveBetService = new MoveBetService();
        }

        public ActionResult Index()
        {
            return ReAuthorize("~/Views/Home/Index.cshtml");
        }

        public ActionResult IndexIpad()
        {
            return ReAuthorize("~/Views/Home/IndexIpad.cshtml");
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
            SessionService.TurnOffIsFirstLoad();
            return View(defaultView);
        }

        private string LoadNotificationContent()
        {
            if (SessionService.IsFirstLoad == false)
            {
                return string.Empty;
            }

            var moveBet = _moveBetService.GetMoveBet();
            var endDate = AppSettings.MoveBetDayEnd ?? moveBet.EstimatedTime;

            if (!endDate.HasValue || DateTime.Now > endDate)
            {
                return string.Empty;
            }

            var startDate = AppSettings.MoveBetDayStart ?? moveBet.AvailableWinlostDate;
            var language = SessionService.UserSession.Language;

            var filePath = Server.MapPath(string.Format(@"~/App_Data/Notification/{0}.htm", language));
            if (!language.Equals(DefaultLanguage, System.StringComparison.OrdinalIgnoreCase) && !System.IO.File.Exists(filePath))
            {
                filePath = filePath.Replace(language, DefaultLanguage);
            }

            using (var reader = new StreamReader(filePath))
            {
                var content = reader.ReadToEnd();
                content = content.Replace("{startDate}", startDate.ToString("MM/dd/yyyy"));
                content = content.Replace("{endDate}", endDate.Value.ToString("HH:mm:ss MM/dd/yyyy"));
                return content;
            }
        }
    }
}