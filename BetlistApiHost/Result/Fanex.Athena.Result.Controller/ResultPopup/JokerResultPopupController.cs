using Fanex.Result.Core.Joker;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class JokerResultPopupController : BaseResultController
    {
        private readonly IJokerResult _resultApi;

        public JokerResultPopupController(IJokerResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gamePlayId, string gameCode, string languageCode)
        {
            return _resultApi.GetJokerResult(custId, gamePlayId, gameCode, languageCode);
        }
    }
}