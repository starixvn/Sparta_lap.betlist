namespace Fanex.Athena.Result.Controller.ResultPopup
{
    using System.Web.Mvc;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;
    using MetaModel;

    public class GPILiveCasinoResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _resultApi;

        public GPILiveCasinoResultPopupController(IThirdPartyResult resultApi = null)
        {
            _resultApi = resultApi ?? new ThirdPartyResult("GPILiveCasinoResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId,
                metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId,
                EndTime = metaModel.WinLostDate
            };

            return _resultApi.GetResultGET(request);
        }
    }
}