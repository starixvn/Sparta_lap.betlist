namespace Fanex.Athena.Controllers.BetList
{
    using System.Web.Mvc;

    public class BettingTicketsCenterController : BaseController
    {
        public ActionResult MultipleCustomers()
        {
            return View("~/Views/BetList/BettingTicketsCenter/Main.cshtml");
        }

        public ActionResult SingleCustomer()
        {
            return View("~/Views/BetList/BettingTicketsCenter/Main.cshtml");
        }

        public ActionResult RefNo()
        {
            return View("~/Views/BetList/BettingTicketsCenter/RefNo.cshtml");
        }

        public ActionResult Running()
        {
            return View("~/Views/BetList/BettingTicketsCenter/Main.cshtml");
        }
    }
}