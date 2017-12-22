namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Model.Report.ViewModel;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ResultAmendmentNotificationReport)]
    public class NotificationReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new NotificationViewModel
            {
                LicenseeName = SessionService.LicenseeSession.LicenseeName
            };

            return ViewForNewUI("~/Views/Report/Notification/Notification.cshtml", model, "NotificationReport");
        }
    }
}