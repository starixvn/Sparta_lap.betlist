using Fanex.Result.Core.Habanero;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class AdvantPlayMiniResultPopupController : BaseResultController
    {
        private readonly IHabaneroResult _resultApi;

        public AdvantPlayMiniResultPopupController(IHabaneroResult resultApi)
        {
            _resultApi = new HabaneroResult("AdvantPlayMiniResultConfigs");
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string gameId3rd, string siteId, string languageCode)
        {
            return _resultApi.GetResult(custId, transId3rd, gameId3rd, siteId, languageCode);
        }
    }
}