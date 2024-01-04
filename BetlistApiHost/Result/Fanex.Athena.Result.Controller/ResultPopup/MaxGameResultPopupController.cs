using System.Web.Mvc;
using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class MaxGameResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public MaxGameResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("MaxGameResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId,
                EndTime = metaModel.WinLostDate
            };

            return _resultApi.GetResultGET(request);
        }
    }
}