namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.PP;

    public class PPResultPopupController : Controller
    {
        private readonly IPPResult _resultApi;

        public PPResultPopupController(IPPResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpGet]
        public string Index(string custId, string gamePlayId, string languageCode)
        {
            return _resultApi.GetPPResult(custId, gamePlayId, languageCode);
        }
    }
}