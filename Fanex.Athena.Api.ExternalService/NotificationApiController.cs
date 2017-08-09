namespace Fanex.Athena.Api.ExternalService
{
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;

    [AllowAnonymous]
    [ApiOneAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NotificationApiController : ApiController
    {
        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        public NotificationApiController()
        {
            _moveBetService = new MoveBetService();
            _languageService = new LanguageService();
        }

        [HttpGet]
        public string GetMoveBetNotification()
        {
            var language = _languageService.GetCurrentLanguage();

            return _moveBetService.GetMoveBetNotification(language);
        }
    }
}