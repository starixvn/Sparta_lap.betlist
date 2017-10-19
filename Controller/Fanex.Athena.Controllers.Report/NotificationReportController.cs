namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Model.Report.ViewModel;
    using Service.Report;
    using Service.Report.Interfaces;

    public class NotificationReportController : ReportBaseController
    {
        private readonly INotificationReportService _notificationReportService;

        public NotificationReportController()
        {
            _notificationReportService = new NotificationReportService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new NotificationViewModel
            {
                NotificationTypes = _notificationReportService.GetNotificationTypes()
            };

            return View("~/Views/Report/Notification/Notification.cshtml", model);
        }

        [HttpGet]
        public ActionResult ResultAmendment()
        {
            var model = new ResultAmendmentViewModel();

            return View("~/Views/Report/Notification/ResultAmendment.cshtml", model);
        }
    }
}