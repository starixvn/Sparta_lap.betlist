namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Extension.Utility;
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
                mainData.Add(new NotificationMessage { Content = i.ToString(), DateTime = DateTime.Today, Status = i % 2 });
            }

            return mainData;
        }

        [HttpPost]
        public dynamic Index(NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new
                {
                    Data = new List<Notification>()
                };
            }

            metaModel.UserId = SessionService.UserSession.UserId;
            metaModel.LicenseeName = SessionService.LicenseeSession.LicenseeName;
            metaModel.Language = Util.MapLanguages.GetValueOrDefault(SessionService.UserSession.Language, "en");

            var mainData = _notificationReportService.GetNotifications(metaModel);

            return new
            {
                Data = mainData.DataList
            };
        }

        [HttpPost]
        public dynamic ResultAmendment(NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new
                {
                    Data = new List<Notification>()
                };
            }

            var mainData = _notificationReportService.GetResultAmendment();

            return new
            {
                Data = mainData
            };
        }
    }
}