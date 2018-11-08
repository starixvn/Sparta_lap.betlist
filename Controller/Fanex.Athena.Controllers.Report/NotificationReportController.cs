namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Web.Mvc;
    using Extension.Helper;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Report.ViewModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Web.ActionFilter;

    [FunctionAuthorization(PermissionKeys.ResultAmendmentNotificationReport)]
    public class NotificationReportController : ReportBaseController
    {
        public NotificationReportController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService)
        : base(languageService, sessionService, appSetting, productService)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new NotificationViewModel
            {
                FromDate = DateTimeHelper.GetFirstDateOfWeek(DateTime.Now, DayOfWeek.Sunday),
                ToDate = DateTime.Now,
                LicenseeName = SessionService.LicenseeSession.LicenseeName
            };

            return View("~/Views/Desktop/Report/Notification/Notification.cshtml", model);
        }
    }
}