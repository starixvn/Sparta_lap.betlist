namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.SAGaming;

    public class SAGamingResultPopupController : BaseResultController
    {
        private readonly ISAGamingResultBuilder _resultApi;

        public SAGamingResultPopupController(ISAGamingResultBuilder resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string transDesc, string matchCode, string betType, string languageCode)
        {
            return _resultApi.Render(transDesc, matchCode, betType, languageCode);
        }
    }
}