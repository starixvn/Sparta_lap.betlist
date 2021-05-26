using System.Web.Mvc;
using Fanex.Result.Core.BG;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class BGGResultPopupController : BaseResultController
    {
        private readonly IBigGamingResult _resultApi;

        public BGGResultPopupController(IBigGamingResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string transId3rd, string currencyId, string siteId, string languageCode)
            => _resultApi.GetResult(custId, transId3rd, currencyId, siteId, languageCode);
    }
}
