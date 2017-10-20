namespace Fanex.Athena.Api.Report
{
    using System.Collections.Generic;
    using System.Web.Http;
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

            var mainData = _notificationReportService.GetNotifications();

            return new
            {
                Data = mainData
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

            var mainData = _notificationReportService.GetNotifications();

            return new
            {
                Data = mainData
            };
        }
    }
}