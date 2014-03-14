// -----------------------------------------------------------------------
// <copyright file="FooterController.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class FooterController:Controller
    {
        public ActionResult Footer()
        {
            return View("~/Views/Footer/Index.cshtml");
        }
    }
}
