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
        private readonly IBetListApiClient _betListApiClient;

        public PPResultPopupController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IBetListApiClient betListApiClient)
            : base(languageService, sessionService, appSetting)
        {
            _betListApiClient = betListApiClient;
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
            return await _betListApiClient.PostAsyncString("/PPResultPopup/Index", model).ConfigureAwait(false);
        }
    }
}