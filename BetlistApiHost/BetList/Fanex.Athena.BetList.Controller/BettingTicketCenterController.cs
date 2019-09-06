using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Controller.CustomActionResult;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Resources;

namespace Fanex.Athena.BetList.Controller
{
    public class BettingTicketCenterController : BaseController
    {
        private readonly IBettingTicketCenterService _service;

        private readonly IBettingTicketCenterExcel _bettingTicketCenterExcel;
        private readonly ITicketBuilderService _ticketBuilderService;

        public BettingTicketCenterController(
            IBettingTicketCenterService service,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            ITicketBuilderService ticketBuilderService
            )
        {
            _service = service;
            _bettingTicketCenterExcel = bettingTicketCenterExcel;
            _ticketBuilderService = ticketBuilderService;

        }

        [HttpPost]
        public async Task<ActionResult> GetBySingleCustomer(SingleCustomersBetListMetaModel metaModel)
        {
            var ticketResult = await _service.GetBetListForSingleCustomerAsync(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

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
                Data = _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom),
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
            var ticketResult = await _service.GetBetListByRefnoAsync(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

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
                Data = _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom)
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> GetByRunning(BaseBettingTicketMetaModel metaModel)
        {
            var ticketResult = await _service.GetRunningBetListAsync(metaModel, metaModel.UserCurrencyList).ConfigureAwait(false);

            if (metaModel.IsExport)
            {
                var excelModel = new BettingTicketCenterExcelModel
                {
                    Data = ticketResult,
                    Option = new BettingTicketCenterExcelOption
                    {
                        ShowTotal = true,
                        ShowSumStake = true,
                        TotalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}"
                    },
                    CustomerCommVisible = false,
                    Title = _service.GetTitle(metaModel)
                };
                return Export(excelModel);
            }

            var result = new
            {
                Data = _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom),
                Total = new
                {
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        private ActionResult Export(BettingTicketCenterExcelModel excelModel)
        {
            if (excelModel.Data.MainTicketList.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var workbook = _bettingTicketCenterExcel.Export(excelModel);
            BetListHelper.ClearData(excelModel.Data);
            excelModel.Data = null;
            return new ExcelResult(workbook);
        }
    }
}