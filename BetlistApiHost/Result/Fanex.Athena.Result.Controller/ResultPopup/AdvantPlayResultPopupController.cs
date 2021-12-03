using Fanex.Result.Core.Habanero;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class AdvantPlayResultPopupController : BaseResultController
    {
        private readonly IHabaneroResult _resultApi;

        public AdvantPlayResultPopupController(IHabaneroResult resultApi)
        {
            _resultApi = new HabaneroResult("AdvantPlayResultConfigs");
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string gameId3rd, string siteId, string languageCode)
        {
            return _resultApi.GetResult(custId, transId3rd, gameId3rd, siteId, languageCode);
        }
    }
}