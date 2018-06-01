namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;

    public class BettingTicketsCenterReportController : BaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/Report/BettingTicketsCenter/Main.cshtml");
        }
    }
}