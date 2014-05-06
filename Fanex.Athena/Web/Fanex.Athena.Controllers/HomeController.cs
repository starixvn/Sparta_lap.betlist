// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;

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