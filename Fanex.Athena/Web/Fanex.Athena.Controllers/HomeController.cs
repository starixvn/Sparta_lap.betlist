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

        public string GetLeauge(int id)
        {
            return Fanex.Athena.Models.Cache.CacheRef.GetLeagueName(id);
        }

        public string GetOutRightName(int id)
        {
            return Fanex.Athena.Models.Cache.CacheRef.GetTeamName(id);
        }

        public string GetLCTeamName(int id)
        {
            return Fanex.Athena.Models.Cache.CacheRef.GetCasinoBetItem(id);
        }

        public ActionResult Footer()
        {
            return View("~/Views/Home/Footer.cshtml");
        }
    }
}