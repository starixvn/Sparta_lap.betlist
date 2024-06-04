using System.Web.Mvc;
using Fanex.Result.Core.SexyGaming;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SexyGamingResultPopupController : BaseResultController
    {
        private readonly ISexyGamingResultBuilder _resultApi;

        public SexyGamingResultPopupController(ISexyGamingResultBuilder resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string transDesc, string matchCode, int betType, string languageCode)
        {
            return _resultApi.Render(transDesc, matchCode, betType, languageCode);
        }
    }
}