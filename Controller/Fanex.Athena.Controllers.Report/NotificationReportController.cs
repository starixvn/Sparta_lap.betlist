namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Model.Report.ViewModel;
    using Resources;

    public class NotificationReportController : ReportBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = CreateViewModel();

            return View("~/Views/Report/Notification/Notification.cshtml", model);
        }

        private static NotificationViewModel CreateViewModel()
        {
            var viewModel = new NotificationViewModel
            {
                NotificationTypes = new List<SelectListItem>
                {
                    new SelectListItem { Value = NotificationType.All.ToString(), Text = ViewResources.All, Selected = true },
                    new SelectListItem { Value = NotificationType.Public.ToString(), Text = ViewResources.Public },
                    new SelectListItem { Value = NotificationType.Special.ToString(), Text = ViewResources.Special },
                    new SelectListItem { Value = NotificationType.Personal.ToString(), Text = ViewResources.Personal },
                    new SelectListItem { Value = NotificationType.MatchDelayNew.ToString(), Text = ViewResources.MatchDelayNew },
                    new SelectListItem { Value = NotificationType.MatchDelayDone.ToString(), Text = ViewResources.MatchDelayDone },
                }
            };

            return viewModel;
        }
    }
}