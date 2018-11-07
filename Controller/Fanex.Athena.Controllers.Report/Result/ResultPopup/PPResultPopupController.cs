namespace Fanex.Athena.Controllers.Report.Result.ResultPopup
{
    using System.Web.Mvc;
    using Extension.Utility;
    using Fanex.Result.Core.PP;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class PPResultPopupController : BaseController
    {
        private readonly IPPResult _resultApi;

        public PPResultPopupController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IPPResult resultApi)
            : base(languageService, sessionService, appSetting)
        {
            _resultApi = resultApi;
        }

        [HttpGet]
        public string Index(string custId, string gamePlayId)
        {
            var languageCode = SessionService.UserSession.Language;
            return _resultApi.GetPPResult(custId, gamePlayId, languageCode);
        }
    }
}