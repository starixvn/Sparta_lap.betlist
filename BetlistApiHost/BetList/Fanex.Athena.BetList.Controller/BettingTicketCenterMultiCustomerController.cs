using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Fanex.Athena.BetList.Controller.CustomActionResult;
using Fanex.Athena.BetList.Excel;
using Fanex.Athena.BetList.Excel.Builders;
using Fanex.Athena.BetList.Excel.Model;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Extension.Utility.Interfaces;
using Fanex.Athena.Model.Core.DataDomain;
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
        public ActionResult GetData(MultipleCustomersBetListMetaModel metaModel)
        {
            if (metaModel.IsExport)
            {
                var totalRow = _service.GetTotalRowForMultipleCustomers(metaModel);
                if (totalRow > MaxRowSupportExportExcel)
                {
                    return Json(new
                    {
                        ErrorCode = RowsExceedLimitErrorCode
                    });
                }

                return Export(metaModel, metaModel.UserCurrencyList, totalRow);
            }

            var ticketResult = _service.GetBetListForMultipleCustomers(metaModel, metaModel.UserCurrencyList);

            return GenerateHtmlViewModel(metaModel, ticketResult, metaModel.HasPlayerCom);
        }

        private ActionResult Export(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> userCurrencyList, int totalRow)
        {
            var title = _service.GetTitle(metaModel);

            if (totalRow <= MaxRowPerFile)
            {
                var normalTicketResult = _service.GetBetListForMultipleCustomers(metaModel, userCurrencyList);
                if (normalTicketResult.MainTicketList.Count == 0)
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.NoContent);
                }

                return ExportXls(normalTicketResult, title, metaModel.HasPlayerCom);
            }

            var ticketResult = _service.GetBetListForMultipleCustomers(metaModel, MaxRowPerFile, userCurrencyList);

            return ExportZip(ticketResult, title, metaModel.HasPlayerCom);
        }

        private JsonResult GenerateHtmlViewModel(MultipleCustomersBetListMetaModel metaModel, TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            var result = new
            {
                Data = _service.BuildTicketText(ticketResult, metaModel, isAllowedCustomerComm),
                Total = new
                {
                    SumWinLoss = _service.BuildSumWinLoss(ticketResult, isAllowedCustomerComm),
                    SumLicenseeWL = (ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat),
                    SumStake = ticketResult.SumStake.ToString(FormatKey.DecimalFormat)
                },
                TotalRows = ticketResult.TotalRows
            };
            ClearData(ticketResult);
            return Json(result);
        }

        private ExcelResult ExportXls(TicketResult ticketResult, string title, bool hasPlayerComm)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                CustomerCommVisible = hasPlayerComm,
                Title = title
            };

            var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
            var workbook = excel.Export(excelModel);
            excelModel.Data = null;
            ClearData(ticketResult);
            return new ExcelResult(workbook);
        }

        private BettingTicketCenterExcelModel CreateBettingTicketCenterExcelModel(GroupTicketResult groupData, string dataPath, string title, bool hasPlayerComm)
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
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                CustomerCommVisible = hasPlayerComm,
                Title = title
            };
        }

        private FileContentResult ExportZip(GroupTicketResult ticketResult, string title, bool hasPlayerComm)
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
                        var excelModel = CreateBettingTicketCenterExcelModel(ticketResult, ticketResult.DataPath[i], title, hasPlayerComm);
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