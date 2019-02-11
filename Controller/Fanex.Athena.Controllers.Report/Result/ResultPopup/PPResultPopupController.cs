namespace Fanex.Athena.Controllers.Report.Result.ResultPopup
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Extension.Utility;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    public class PPResultPopupController : BaseController
    {
        private readonly IBridgeClientService _bridgeClientService;

        public PPResultPopupController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IBridgeClientService bridgeClientService)
            : base(languageService, sessionService, appSetting)
        {
            _bridgeClientService = bridgeClientService;
        }

        [HttpGet]
        public async Task<string> Index(string custId, string gamePlayId)
        {
            var model = new
            {
                custId,
                gamePlayId,
                languageCode = SessionService.UserSession.Language
            };
            var url = new Uri($"http://localhost:55050/PPResultPopup/Index");
            return await _bridgeClientService.PostAsyncString(url, model).ConfigureAwait(false);
        }
    }
}