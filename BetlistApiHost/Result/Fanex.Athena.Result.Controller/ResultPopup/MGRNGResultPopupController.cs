using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class MGRNGResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public MGRNGResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("MGResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            return _resultApi.GetResultGET(request);
        }
    }
}