// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Users;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : BaseController
    {
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
            var user = SessionService.UserSession;
            if (user == null || user.UserId <= 0
                || string.IsNullOrEmpty(user.UserName) || SessionService.LicenseeSession == null)
            {
                Session.Clear();
                return RedirectToAction("LocationRestriction", "Utility");
            }

            return View(defaultView);
        }
    }
}