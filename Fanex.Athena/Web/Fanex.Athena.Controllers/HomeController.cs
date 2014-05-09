// -----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;
    using Fanex.Athena.Extension.Utility;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class HomeController : ReadonlyBaseController
    {
        public ActionResult Index()
        {
            ReAuthorize();
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult IndexIpad()
        {
            ReAuthorize();
            return View("~/Views/Home/IndexIpad.cshtml");
        }

        private void ReAuthorize()
        {
            var user = SessionService.UserSession;
            if (user == null || user.UserId <= 0
                || string.IsNullOrEmpty(user.UserName))
            {
                Session.RemoveAll();
                Session.Clear();
                RedirectToAction("LocationRestriction", "Utility");
            }
        }

    }
}