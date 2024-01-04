using System;
using System.Web.Mvc;
using Fanex.Result.Core.SportsLottery;

namespace Fanex.Athena.Result.Controller.ResultPopup
{
    public class SportsLotteryResultMainPageController : BaseResultController
    {
        private readonly ISportsLotteryResult _resultApi;

        public SportsLotteryResultMainPageController(ISportsLotteryResult resultApi)
        {
            _resultApi = resultApi ?? new SportsLotteryResult();
        }

        [HttpPost]
        public string ResultPage(string platform, string languageCode)
        {
            var isDesktop = string.Equals(platform, "d", StringComparison.OrdinalIgnoreCase);
            return _resultApi.GetResultPage(isDesktop, languageCode);
        }
    }
}