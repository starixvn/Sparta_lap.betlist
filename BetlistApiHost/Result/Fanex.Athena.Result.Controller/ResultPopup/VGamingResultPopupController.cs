namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.VGaming;

    public class VGamingResultPopupController : BaseResultController
    {
        private readonly IVGamingResult _resultApi;

        public VGamingResultPopupController(IVGamingResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gameRoundId, string gameId, string siteId, int sportType, string languageCode)
        {
            return _resultApi.GetResult(custId, gameRoundId, gameId, siteId, sportType, languageCode);
        }
    }
}