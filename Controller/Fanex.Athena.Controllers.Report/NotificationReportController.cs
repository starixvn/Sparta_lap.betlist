namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Model.Report.ViewModel;

    public class NotificationReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new NotificationViewModel();

            return View("~/Views/Report/Notification/Notification.cshtml", model);
        }
    }
}