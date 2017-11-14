namespace Fanex.Athena.Api.ExternalService
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;
    using Service.Report;
    using Service.Report.Interfaces;

    [AllowAnonymous]
    [ApiOneAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationApiController : ApiController
    {
        private static readonly DateTime MinDate = new DateTime(1990, 1, 1);

        private static readonly DateTime MaxDate = new DateTime(3000, 1, 1);

        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        private readonly INotificationReportService _notificationReportService;

        public NotificationApiController()
        {
            _moveBetService = new MoveBetService();
            _languageService = new LanguageService();
            _notificationReportService = new NotificationReportService();
        }

        [HttpGet]
        public string Index()
        {
            var language = _languageService.GetCurrentLanguage();

            return _moveBetService.GetMoveBetNotification(language);
        }

        [HttpGet]
        public dynamic GetNotificationMessages([FromUri]NotificationMetaModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return new ReportResultModel<ResultAmendment>
                {
                    DataList = new List<ResultAmendment>(),
                    TotalRows = 0
                };
            }

            metaModel.IsOnlyUnread = true;
            metaModel.FromDate = MinDate;
            metaModel.ToDate = MaxDate;
            metaModel.PageSize = 10;
            metaModel.PageIndex = 1;

            return _notificationReportService.GetNotificationMessages(metaModel);
        }

        [HttpPost]
        public dynamic MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel)
        {
            var result = _notificationReportService.MarkNotificationAsRead(metaModel);

            return new
            {
                Code = result.Code,
                Message = result.Message,
                IsSuccessful = result.IsSuccessful
            };
        }
    }
}