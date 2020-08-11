using System.Web.Mvc;
using Fanex.Result.Core.PGSoft;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class PGSoftResultPopupController : BaseResultController
    {
        private readonly IPGSoftResult _resultApi;

        public PGSoftResultPopupController(IPGSoftResult resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string Index(string custId, string gamePlayId, string gameId3rd, string siteId, string languageCode)
        {
            var param = new PGSoftParam
            {
                CustId = custId,
                GamePlayId = gamePlayId,
                GameId3rd = gameId3rd,
                SiteId = siteId,
                Language = languageCode
            };

            return _resultApi.GetResult(param);
        }
    }
}
