namespace Fanex.Athena.Api.ExternalService
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
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
        private const int MaximumMonth = 6;

        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        private readonly INotificationService _notificationService;

        public NotificationApiController() : this(null, null, null)
        {
        }

        public NotificationApiController(IMoveBetService moveBetService, ILanguageService languageService, INotificationService notificationService)
        {
            _moveBetService = moveBetService ?? new MoveBetService();
            _languageService = languageService ?? new LanguageService();
            _notificationService = notificationService ?? new NotificationService();
        }

        private string UserName => (string)ActionContext.ActionArguments["username"];

        private string SiteName => (string)ActionContext.ActionArguments["sitename"];

        [HttpGet]
        public string Index()
        {
            var language = _languageService.GetCurrentLanguage();

            return _moveBetService.GetMoveBetNotification(language);
        }

        [HttpGet]
        public dynamic GetNotificationMessages([FromUri]NotificationMetaModel metaModel)
        {
            metaModel.IsOnlyUnread = true;
            metaModel.FromDate = DateTime.Today.AddMonths(-MaximumMonth);
            metaModel.ToDate = DateTime.Today.AddDays(1);
            metaModel.PageSize = 10;
            metaModel.PageIndex = 1;
            metaModel.UserName = UserName;
            metaModel.LicenseeName = SiteName;

            return _notificationService.GetNotificationMessages(metaModel);
        }

        [HttpPost]
        public dynamic MarkNotificationAsRead(NotificationMarkAsReadMetaModel metaModel)
        {
            metaModel.UserName = UserName;
            metaModel.LicenseeName = SiteName;

            var result = _notificationService.MarkNotificationAsRead(metaModel);

            return new
            {
                Code = result.Code,
                Message = result.Message,
                IsSuccessful = result.IsSuccessful
            };
        }
    }
}