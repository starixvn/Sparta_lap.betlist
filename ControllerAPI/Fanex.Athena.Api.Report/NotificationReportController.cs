namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Extension.Utility;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Report;
    using Service.Report.Interfaces;

    public class NotificationReportController : ApiReportBaseController
    {
        private readonly INotificationReportService _notificationReportService;

        public NotificationReportController()
        {
            _notificationReportService = new NotificationReportService();
        }

        [HttpGet]
        public dynamic GetNotificationMessages()
        {
            var mainData = new List<NotificationMessage>();

            var random = new Random();

            var length = random.Next(0, 5);
            for (int i = 0; i < length; i++)
            {
                mainData.Add(new NotificationMessage { Content = i.ToString(), DateTime = DateTime.Today });
            }

            return mainData;
        }

        [HttpPost]
        public dynamic Index(NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<Notification>
                {
                    DataList = new List<Notification>(),
                    TotalRows = 0
                };
            }

            UpdateMetaModel(metaModel);

            return _notificationReportService.GetNotifications(metaModel);
        }

        [HttpPost]
        public dynamic ResultAmendment(NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<ResultAmendment>
                {
                    DataList = new List<ResultAmendment>(),
                    TotalRows = 0
                };
            }

            UpdateMetaModel(metaModel);

            return _notificationReportService.GetResultAmendment(metaModel);
        }

        private void UpdateMetaModel(NotificationMetaModel metaModel)
        {
            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeName = SessionService.LicenseeSession.LicenseeName;
            metaModel.Language = Util.MapLanguages.GetValueOrDefault(SessionService.UserSession.Language, "en");
        }
    }
}