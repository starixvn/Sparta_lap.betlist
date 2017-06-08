namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;

    public class BonusBetListController : BetListBaseController
    {
        [HttpGet]
        public ActionResult Detail()
        {
            return View("~/Views/BetList/BonusDetail/Main.cshtml");
        }
    }
}