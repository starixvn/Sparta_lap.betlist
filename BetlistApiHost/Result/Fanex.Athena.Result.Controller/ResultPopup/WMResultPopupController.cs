using System.Web.Mvc;
using Fanex.Result.Core.WM;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class WMResultPopupController : BaseResultController
    {
        private readonly IWMResultBuilder _resultApi;

        public WMResultPopupController(IWMResultBuilder resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string transDesc, string gameId3rd, int betType, string languageCode)
        {
            return _resultApi.Render(transDesc, gameId3rd, betType, languageCode);
        }
    }
}