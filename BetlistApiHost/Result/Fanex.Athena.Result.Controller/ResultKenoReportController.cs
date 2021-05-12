namespace Fanex.Athena.Result.Controller
{
    using Service.Interfaces;
    using System;
    using System.Web.Mvc;

    public class ResultKenoReportController : BaseResultController
    {
        private readonly IKenoLotteryTGResultFactory _resultApi;

        public ResultKenoReportController(IKenoLotteryTGResultFactory resultApi)
        {
            _resultApi = resultApi;
        }

        [HttpPost]
        public string GetResultPage(int vendorProductId, string custId, string languageCode, bool isDesktop = true)
        {
            var result = _resultApi.CreateResultApi(vendorProductId);

            return result.GetResult(custId, isDesktop, DateTime.Now, Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme), languageCode);
        }
    }
}