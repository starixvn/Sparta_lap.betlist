using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.SportsLottery;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;
using System;
using System.Web.Mvc;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SportsLotteryResultPopupController : BaseResultController
    {
        private readonly IThirdPartyResult _thirdPartyResultApi;

        public SportsLotteryResultPopupController(IThirdPartyResult thirdPartyResultApi = null)
        {
            _thirdPartyResultApi = thirdPartyResultApi ?? new ThirdPartyResult("SportsLotteryDetailResultConfigs");
        }

        [HttpPost]
        public string Index(ThirdPartyMetaModel metaModel)
        {
            const string defaultPlatform = "D";

            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                Platform = defaultPlatform,
                BetTypeId = metaModel.BetTypeId
            };

            return _thirdPartyResultApi.GetResultGET(request);
        }

        [HttpPost]
        public string ResultPage(string platform, string languageCode)
        {
            var resultAPI = new SportsLotteryResult();

            var isDesktop = string.Equals(platform, "d", StringComparison.OrdinalIgnoreCase);
            return resultAPI.GetResultPage(isDesktop, languageCode);
        }
    }
}