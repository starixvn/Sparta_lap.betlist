using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Utility.Helpers;

namespace Fanex.Athena.BetList.Controller
{
    public class LiveReportBetListController : BaseController
    {
        private readonly IBetListService _betListService;

        public LiveReportBetListController(IBetListService betListService)
        {
            _betListService = betListService;
        }

        [HttpPost]
        public async Task<JsonResult> GetData(LiveReportBetListMetaModel metaModel)
        {
            var ticketResult = await _betListService.GetBetListData(metaModel).ConfigureAwait(false);

            var result = new
            {
                Main = _betListService.BuildTicketTextList(ticketResult),
                Total = new
                {
                    SumWinLoss = ticketResult.SumWinLoss + ticketResult.SumPlayerComm,
                    SumLicenseeWl = ticketResult.SumLicenseeWL + ticketResult.SumComm
                }
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> GetPagingData(LiveReportBetListMetaModel metaModel)
        {
            var ticketResult = await _betListService.GetBetListData(metaModel).ConfigureAwait(false);

            var totalPage = metaModel.TotalRows % metaModel.PageSize == 0
                ? metaModel.TotalRows / metaModel.PageSize
                : (metaModel.TotalRows / metaModel.PageSize) + 1;
            var startIndex = ((metaModel.PageIndex - 1) * metaModel.PageSize) + 1;

            var sumLicenseeWl = metaModel.SumLicenseeWL + metaModel.SumComm;
            var sumWinLossText = NumberHelper.DecFormat(metaModel.SumWinlost + metaModel.SumPlayerComm, 2);
            var sumLicenseeWlText = NumberHelper.DecFormat(sumLicenseeWl, 2);
            sumLicenseeWlText = sumLicenseeWl < 0
                ? string.Format("<span class='negative-number'>{0}</span>", sumLicenseeWlText)
                : sumLicenseeWlText;

            var result = new
            {
                page = metaModel.PageIndex,
                total = totalPage,
                records = metaModel.TotalRows,
                rows = _betListService.BuildTicketTextList(ticketResult, metaModel, startIndex),
                userdata = new
                {
                    WinLoss = sumWinLossText,
                    LicenseeWL = sumLicenseeWlText
                },
                IsHasCustComm = metaModel.HasPlayerCom
            };

            BetListHelper.ClearData(ticketResult);

            return Json(result);
        }
    }
}