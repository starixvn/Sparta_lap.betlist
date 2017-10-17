namespace Fanex.Athena.Controllers.Report
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.Core.DataDomain;
    using Model.Report.ViewModel;
    using Newtonsoft.Json.Linq;
    using Resources;
    using Service.Core.Helpers;
    using Service.Core.Utility;
    using Service.Core.Utility.Interfaces;

    public class NotificationReportController : ReportBaseController
    {
        private readonly IWebClientService _webClientService;

        public NotificationReportController()
        {
            _webClientService = new WebClientService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = CreateViewModel();

            return View("~/Views/Report/Notification/Notification.cshtml", model);
        }

        private NotificationViewModel CreateViewModel()
        {
            var viewModel = new NotificationViewModel
            {
                NotificationTypes = GetNotificationType()
            };

            return viewModel;
        }

        private Dictionary<string, string> GetNotificationType()
        {
            var notificationTypeUrl = AppSettings.GetValue("NotificationTypeUrl");

            var json = string.Empty;
            try
            {
                json = _webClientService.DownloadString(notificationTypeUrl);
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception, "ApiUrl: " + notificationTypeUrl);
                return new Dictionary<string, string>();
            }

            if (string.IsNullOrEmpty(json))
            {
                return new Dictionary<string, string>();
            }

            var jobject = JObject.Parse(json);
            if (jobject.HasValues)
            {
                jobject.AddFirst(new JProperty("0", ViewResources.All));
            }

            return jobject.ToObject<Dictionary<string, string>>();
        }
    }
}