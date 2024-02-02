using System;
using System.Web.Mvc;
using Fanex.Athena.Result.Controller.MetaModel;
using Fanex.Result.Core.LinkedGames;
using Fanex.Result.Core.ThirdParty;
using Fanex.Result.Core.ThirdParty.Models;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class Happy5LinkedGamesResultPopupController : BaseResultController
    {
        private readonly ILinkedGamesResult _resultApi;
        private readonly IThirdPartyResult _thirdPartyResultApi;

        public Happy5LinkedGamesResultPopupController(ILinkedGamesResult resultApi, IThirdPartyResult thirdPartyResultApi = null)
        {
            _resultApi = resultApi;
            _thirdPartyResultApi = thirdPartyResultApi ?? new ThirdPartyResult("LinkedGamesResultConfigs");
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
        public string Soccer5(string matchId, string balls, string platform, string languageCode)
        {
            var isDesktop = string.Equals(platform, "d", StringComparison.OrdinalIgnoreCase);
            return _resultApi.GetSoccer5Result(matchId, balls, isDesktop, language: languageCode);
        }
    }
}