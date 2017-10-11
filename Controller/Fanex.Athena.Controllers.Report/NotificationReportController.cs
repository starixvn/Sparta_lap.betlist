namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;

    public class NotificationReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Report/Notification/Notification.cshtml");
        }
    }
}