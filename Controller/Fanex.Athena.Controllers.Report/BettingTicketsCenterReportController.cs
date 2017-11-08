namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;

    public class BettingTicketsCenterReportController : ReportBaseController
    {
        public ActionResult Index()
        {
            return View("~/Views/Report/BettingTicketsCenter/Main.cshtml");
        }
    }
}