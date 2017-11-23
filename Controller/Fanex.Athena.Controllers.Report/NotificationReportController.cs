namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Model.Report.ViewModel;
    using Service.Report;
    using Service.Report.Interfaces;
    using Web.ActionFilter;
    using Model.Core.DataDomain;

    [FunctionAuthorization(PermissionKeys.ResultAmendmentNotificationReport)]
    public class NotificationReportController : ReportBaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationReportController()
        {
            _notificationService = new NotificationService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new NotificationViewModel
            {
                NotificationTypes = _notificationService.GetNotificationTypes()
            };

            return View("~/Views/Report/Notification/Notification.cshtml", model);
        }
    }
}