using System.Web.Mvc;
using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class JokerResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public JokerResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("JokerResultConfigs");
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