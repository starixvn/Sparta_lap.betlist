using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Controller.CustomActionResult;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Resources;

namespace Fanex.Athena.BetList.Controller
{
    public class BettingTicketCenterController : BaseController
    {
        private readonly IBettingTicketCenterService _service;

        private readonly IBettingTicketCenterExcel _bettingTicketCenterExcel;

        public BettingTicketCenterController(
            IBettingTicketCenterService service,
            IBettingTicketCenterExcel bettingTicketCenterExcel)
        {
            _service = service;
            _bettingTicketCenterExcel = bettingTicketCenterExcel;
        }

        [HttpPost]
        public async Task<ActionResult> GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            var ticketResult = await _service.GetBetListForSingleCustomer(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    Option = new BettingTicketCenterExcelOption
                    {
                        ShowTotal = true,
                        ShowSumStake = true,
                        ShowLicenseeWl = true,
                        ShowWinLoss = true,
                        TotalText = ViewResources.Total
                    },
                    CustomerCommVisible = metaModel.HasPlayerCom,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            return Json(new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom),
                Total = new
                {
                    SumWinLoss = _service.BuildSumWinLoss(ticketResult, metaModel.HasPlayerCom),
                    SumLicenseeWL = (ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat),
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            });
        }

        [HttpPost]
        public async Task<ActionResult> GetByRefno(RefNoBetListMetaModel metaModel)
        {
            var ticketResult = await _service.GetBetListByRefno(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    Option = new BettingTicketCenterExcelOption
                    {
                        ShowLicenseeWl = true,
                        ShowWinLoss = true,
                        ShowTotal = false
                    },
                    CustomerCommVisible = metaModel.HasPlayerCom,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            var result = new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom)
            };
            ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var ticketResult = await _service.GetRunningBetList(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    Option = new BettingTicketCenterExcelOption
                    {
                        ShowTotal = true,
                        ShowSumStake = true,
                        TotalText = $"{ViewResources.Totalin} {metaModel.BaseCurrency.ToUpperInvariant()}"
                    },
                    CustomerCommVisible = false,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            var result = new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom),
                Total = new
                {
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
            ClearData(ticketResult);
            return Json(result);
        }

        private ActionResult Export(BettingTicketCenterExcelModel excelModel)
        {
            if (excelModel.Data.MainTicketList.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var workbook = _bettingTicketCenterExcel.Export(excelModel);
            ClearData(excelModel.Data);
            excelModel.Data = null;
            return new ExcelResult(workbook);
        }
    }
}