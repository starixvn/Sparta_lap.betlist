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
            var metaModel = new NotificationMetaModel
            {
                IsOnlyUnread = true,
                FromDate = new DateTime(1990, 1, 1),
                ToDate = new DateTime(3000, 1, 1),
                PageIndex = 1,
                PageSize = 10
            };

            UpdateMetaModel(metaModel);

            return _notificationReportService.GetNotificationMessages(metaModel);
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

            return _notificationReportService.GetGeneral(metaModel);
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

        [HttpPost]
        public dynamic MarkNotificationAsRead(NotificationMessage metaModel)
        {
            var userSession = SessionService.UserSession;

            var result = _notificationReportService.MarkNotificationAsRead(userSession.UserId, SessionService.LicenseeSession.LicenseeName, metaModel);

            return new
            {
                Code = result.Code,
                Message = result.Message,
                IsSuccessful = result.IsSuccessful
            };
        }

        private void UpdateMetaModel(NotificationMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserId = userSession.UserId;
            metaModel.LicenseeName = SessionService.LicenseeSession.LicenseeName;
            metaModel.Language = Util.MapLanguages.GetValueOrDefault(userSession.Language, "en");
        }
    }
}