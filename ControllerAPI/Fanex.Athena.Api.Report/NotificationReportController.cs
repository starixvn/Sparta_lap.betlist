namespace Fanex.Athena.Api.Report
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Base.Attributes;
    using Extension.Utility;
    using Model.Core.DataDomain;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.ResultAmendmentNotificationReport)]
    public class NotificationReportController : ApiReportBaseController
    {
        private static readonly DateTime MinDate = new DateTime(1990, 1, 1);

        private static readonly DateTime MaxDate = new DateTime(3000, 1, 1);

        private readonly INotificationService _notificationService;

        public NotificationReportController()
        {
            _notificationService = new NotificationService();
        }

        public NotificationReportController(
            INotificationService notificationService,
            ISessionService sessionService,
            IProductService productService)
            : base(sessionService, productService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public dynamic GetNotificationMessages()
        {
            var metaModel = new NotificationMetaModel
            {
                IsOnlyUnread = true,
                FromDate = MinDate,
                ToDate = MaxDate,
                PageIndex = 1,
                PageSize = 10
            };

            UpdateMetaModel(metaModel);

            return _notificationService.GetNotificationMessages(metaModel);
        }

        [HttpPost]
        public dynamic Index(NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<GeneralNotification>
                {
                    DataList = new List<GeneralNotification>(),
                    TotalRows = 0
                };
            }

            UpdateMetaModel(metaModel);

            return _notificationService.GetGeneral(metaModel);
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

            var result = _notificationService.GetResultAmendment(metaModel);

            return new
            {
                DataList = result.DataList,
                TotalRows = result.TotalRows,
                DownloadFileApi = result.AdditionalInfo
            };
        }

        [HttpPost]
        public dynamic MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel)
        {
            metaModel.UserName = SessionService.UserSession.UserName;
            metaModel.LicenseeName = SessionService.LicenseeSession.LicenseeName;

            var result = _notificationService.MarkNotificationAsRead(metaModel);

            return new
            {
                Code = result.Code,
                Message = result.Message,
                IsSuccessful = result.IsSuccessful
            };
        }

        [HttpPost]
        public HttpResponseMessage DownLoadFileAttachment(NotificationAttachmentMetaModel metaModel)
        {
            var content = _notificationService.GetFileAttachmentData(metaModel.Api, metaModel.FileName);
            if (content == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(content);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

        private void UpdateMetaModel(NotificationMetaModel metaModel)
        {
            var userSession = SessionService.UserSession;
            metaModel.UserName = userSession.UserName;
            metaModel.LicenseeName = SessionService.LicenseeSession.LicenseeName;
            metaModel.Language = Util.MapLanguages.GetValueOrDefault(userSession.Language, "en");
        }
    }
}