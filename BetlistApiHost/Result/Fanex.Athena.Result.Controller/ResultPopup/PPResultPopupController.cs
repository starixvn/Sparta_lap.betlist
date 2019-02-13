namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Common.Utility.Attributes;
    using Fanex.Result.Core.PP;

    [CustomAuth]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class PPResultPopupController : Controller
    {
        private readonly IPPResult _resultApi;

        public PPResultPopupController(IPPResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gamePlayId, string languageCode)
        {
            return _resultApi.GetPPResult(custId, gamePlayId, languageCode);
        }
    }
}