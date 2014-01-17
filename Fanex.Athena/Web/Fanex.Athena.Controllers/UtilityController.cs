// -----------------------------------------------------------------------
// <copyright file="UtilityController.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Utility controller.
    /// </summary>
    public class UtilityController : ReadonlyBaseController
    {
        public ActionResult AccessDenied()
        {
            return View("~/Views/Utility/AccessDenied.cshtml");
        }
    }
}