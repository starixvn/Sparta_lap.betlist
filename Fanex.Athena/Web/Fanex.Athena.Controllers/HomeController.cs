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
    public class HomeController : ReadonlyBaseController
    {
        public ActionResult Index()
        {
            return Redirect("~/Index.aspx");
        }
    }
}