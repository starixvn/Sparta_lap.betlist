namespace Fanex.Athena.Api.ExternalService
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
    using Model.Report.DTO;
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
    }
}