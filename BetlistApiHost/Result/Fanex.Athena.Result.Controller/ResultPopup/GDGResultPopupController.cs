using Fanex.Result.Core.GDG;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class GDGResultPopupController : BaseResultController
    {
        private readonly IGoldDiamondGamingResult _resultApi;

        public GDGResultPopupController(IGoldDiamondGamingResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string gameId3rd, string siteId, string languageCode)
        {
            return _resultApi.GetResult(custId, transId3rd, gameId3rd, siteId, languageCode);
        }
    }
}