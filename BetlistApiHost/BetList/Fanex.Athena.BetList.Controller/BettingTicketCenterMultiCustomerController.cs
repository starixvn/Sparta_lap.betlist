using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Controller.CustomActionResult;
using Fanex.Athena.BetList.Controller.Utility;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Helpers;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Ionic.Zip;
using Newtonsoft.Json;

namespace Fanex.Athena.BetList.Controller
{
    public class BettingTicketCenterMultiCustomerController : BaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterService _service;

        private readonly IBettingTicketCenterZipBuilder _zipBuilder;

        public BettingTicketCenterMultiCustomerController(
            IAppSettings appSettings,
            IBettingTicketCenterService service,
            IBettingTicketCenterZipBuilder zipBuilder)
        {
            _appSettings = appSettings;
            _service = service;
            _zipBuilder = zipBuilder;
        }

        private int MaxRowSupportExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportExportExcel"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        [HttpPost]
        public async Task<ActionResult> GetData(MultipleCustomersBetListMetaModel metaModel)
        {
            Logging.LogService.Error("btc\n"+ JsonConvert.SerializeObject(metaModel));
            try
            {
                if (metaModel.IsExport)
                {
                    var totalRow = await _service.GetTotalRowForMultipleCustomers(metaModel);
                    if (totalRow > MaxRowSupportExportExcel)
                    {
                        return Json(new
                        {
                            ErrorCode = RowsExceedLimitErrorCode
                        });
                    }

                    return await Export(metaModel, metaModel.UserCurrencyList, totalRow);
                }

                var ticketResult = await _service.GetBetListForMultipleCustomers(metaModel, metaModel.UserCurrencyList);

                return GenerateJsonResult(metaModel, ticketResult, metaModel.HasPlayerCom);
            }
            catch (Exception ex)
            {
                Logging.LogService.Error(ex.Message, ex);
            }finally
            {
                Logging.LogService.Error("end-btc\n");
            }


            return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent);
        }

        protected internal virtual ExcelResult ReturnExcel(BettingTicketCenterExcelModel excelModel, TicketResult ticketResult)
        {
            var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
            var workbook = excel.Export(excelModel);
            excelModel.Data = null;
            BetListHelper.ClearData(ticketResult);
            return new ExcelResult(workbook);
        }

        private async Task<ActionResult> Export(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> userCurrencyList, int totalRow)
        {
            Logging.LogService.Info("BTC- Generate excel file");

            var title = _service.GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrency.ToUpperInvariant()}";

            if (totalRow <= MaxRowPerFile)
            {
                var normalTicketResult = await _service.GetBetListForMultipleCustomers(metaModel, userCurrencyList).ConfigureAwait(true);
                if (normalTicketResult.MainTicketList.Count == 0)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent);
                }

                return ExportXls(normalTicketResult, title, metaModel.HasPlayerCom, totalText);
            }

            var ticketResult = await _service.GetBetListForMultipleCustomers(metaModel, MaxRowPerFile, userCurrencyList).ConfigureAwait(true);

            var zipFile = _zipBuilder.BuildZip(HttpContext.Response, ticketResult, title, metaModel.HasPlayerCom, totalText);
            Logging.LogService.Info("END-BTC- Generate excel file");

            return zipFile == null ? null : ZipContentResult(zipFile);
        }

        private JsonResult GenerateJsonResult(MultipleCustomersBetListMetaModel metaModel, TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var result = new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel, isAllowedCustomerComm),
                Total = new
                {
                    SumWinLoss = _service.BuildSumWinLoss(ticketResult, isAllowedCustomerComm),
                    SumLicenseeWL = (ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat),
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat),
                },
                TotalRows = ticketResult.TotalRows
            };
            BetListHelper.ClearData(ticketResult);
            return Json(result);
        }

        private ExcelResult ExportXls(TicketResult ticketResult, string title, bool hasPlayerComm, string totalText)
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
                    TotalText = totalText
                },
                CustomerCommVisible = hasPlayerComm,
                Title = title
            };

            return ReturnExcel(excelModel, ticketResult);
        }

        protected FileContentResult ZipContentResult(ZipFile zipFile)
        {
            MemoryStream stream = new MemoryStream();

            zipFile.Save(stream);
            var bytes = stream.ToArray();
            return File(bytes, "application/zip", "BettingTicketsCenter_MultiCustomer.zip");
        }
    }
}