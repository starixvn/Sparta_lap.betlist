using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Fanex.Athena.BetList.Controller.CustomActionResult;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Builders;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Common.Constant;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Ionic.Zip;
using Newtonsoft.Json;
using NPOI.SS.UserModel;

namespace Fanex.Athena.BetList.Controller
{
    public class BettingTicketCenterMultiCustomerController : BaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterService _service;
        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterMultiCustomerController(
            IAppSettings appSettings,
            IBettingTicketCenterService service,
            ITempFileService tempFileService)
        {
            _appSettings = appSettings;
            _service = service;
            _tempFileService = tempFileService;
        }

        private int MaxRowSupportExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportExportExcel"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        [HttpPost]
        public async Task<ActionResult> GetData(MultipleCustomersBetListMetaModel metaModel)
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

        protected internal virtual ExcelResult ReturnExcel(BettingTicketCenterExcelModel excelModel, TicketResult ticketResult)
        {
            var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
            var workbook = excel.Export(excelModel);
            excelModel.Data = null;
            ClearData(ticketResult);
            return new ExcelResult(workbook);
        }

        private async Task<ActionResult> Export(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> userCurrencyList, int totalRow)
        {
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

            var ticketResult = await _service.GetBetListForMultipleCustomers(metaModel, MaxRowPerFile, userCurrencyList).ConfigureAwait(false);

            return ExportZip(ticketResult, title, metaModel.HasPlayerCom, totalText);
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
            ClearData(ticketResult);
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

        private BettingTicketCenterExcelModel CreateBettingTicketCenterExcelModel(
            GroupTicketResult groupData,
            string dataPath,
            string title,
            bool hasPlayerComm,
            string totalText)
        {
            var contentData = _tempFileService.ReadString(dataPath);
            if (string.IsNullOrEmpty(contentData))
            {
                return null;
            }

            var dataBySheet = JsonConvert.DeserializeObject<TicketResult>(contentData);

            dataBySheet.SumStake = groupData.SumStake;
            dataBySheet.SumWinLoss = groupData.SumWinLoss;
            dataBySheet.SumLicenseeWL = groupData.SumLicenseeWL;
            dataBySheet.SumBetEffective = groupData.SumBetEffective;
            dataBySheet.SumComm = groupData.SumComm;
            dataBySheet.SumPlayerComm = groupData.SumPlayerComm;
            dataBySheet.TotalRows = groupData.TotalRows;

            return new BettingTicketCenterExcelModel
            {
                Data = dataBySheet,
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
        }

        private FileContentResult ExportZip(GroupTicketResult ticketResult, string title, bool hasPlayerComm, string totalText)
        {
            IWorkbook workbook = null;
            var excelFiles = new List<string>();
            var response = HttpContext.Response;

            for (int i = 0; i < ticketResult.DataPath.Count; i++)
            {
                if (response.IsClientConnected)
                {
                    var builder = DependencyResolver.Current.GetService<IBettingTicketCenterExcelBuilder>();
                    using (var excelService = new BettingTicketCenterExcel(builder))
                    {
                        var excelModel = CreateBettingTicketCenterExcelModel(ticketResult, ticketResult.DataPath[i], title, hasPlayerComm, totalText);
                        if (excelModel != null)
                        {
                            workbook = excelService.Export(excelModel);
                            var dataFileName = ticketResult.DataPath[i] + ".dat";
                            var dataFilePath = Path.Combine(_appSettings.TempFilePath, dataFileName);
                            using (var stream = new FileStream(dataFilePath, FileMode.CreateNew, FileAccess.Write))
                            using (var bufferStream = new BufferedStream(stream))
                            {
                                workbook.Write(bufferStream);
                                excelFiles.Add(dataFileName);
                                workbook.Clear();
                                ClearData(excelModel.Data);
                                excelModel.Data = null;
                            }
                        }
                    }
                }
            }

            CleanDataFile(ticketResult.DataPath);
            return ReturnZipFile(excelFiles, response);
        }

        private FileContentResult ReturnZipFile(List<string> excelFiles, HttpResponseBase response)
        {
            using (var zip = new ZipFile())
            {
                for (var i = 0; i < excelFiles.Count; i++)
                {
                    if (response.IsClientConnected)
                    {
                        zip.AddEntry($"sheet{i}.xls", _tempFileService.ReadBytes(excelFiles[i]));
                    }
                }

                CleanDataFile(excelFiles);
                return response.IsClientConnected ? ZipContentResult(zip) : null;
            }
        }

        private void CleanDataFile(IList<string> dataPath)
        {
            foreach (var item in dataPath)
            {
                _tempFileService.Delete(item);
            }
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