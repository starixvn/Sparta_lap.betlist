namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Mvc;
    using Base;
    using Base.Attributes;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Extension.Utility;
    using Fanex.Athena.Extension.Utility.Interfaces;
    using Ionic.Zip;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Newtonsoft.Json;
    using NPOI.SS.UserModel;
    using Resources;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterMultiCustomerController : BaseApiController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private const int ServerBusyErrorCode = 1002;

        private const int MaxRowSupportExportExcel = 300000;

        private const int MaxRowPerFile = 50;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterService _service;
        private readonly ITempFileService _tempFileService;
        public BettingTicketCenterMultiCustomerController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            IAppSettings appSettings,
            IBettingTicketCenterService service,
            ITempFileService tempFileService)
            : base(sessionService, productService, langService)
        {
            _appSettings = appSettings;
            _service = service;
            _tempFileService = tempFileService;
        }

        [System.Web.Http.HttpPost]
        public dynamic GetData(MultipleCustomersBetListMetaModel metaModel)
        {
            var licenseeSession = SessionService.LicenseeSession;
            if (metaModel.IsExport && 
                _appSettings.IsDisableExportLargeBetList &&
                _appSettings.LicenseesWithLargeData.Contains(licenseeSession.LicenseeId))
            {
                return new
                {
                    ErrorCode = ServerBusyErrorCode,
                    ErrorMsg = ViewResources.ServerBusyMessage
                };
            }

            TicketResult ticketResult;
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            var isAllowedCustomerComm = licenseeSession.IsAllowedCustomerComm;
            if (!metaModel.IsValid || !_service.IsValidCurrencyId(metaModel.CurrencyId, userCurrencyList))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                if (metaModel.IsExport)
                {
                    var totalRow = _service.GetTotalRowForMultipleCustomers(metaModel);
                    if (totalRow > MaxRowSupportExportExcel)
                    {
                        return new
                        {
                            ErrorCode = RowsExceedLimitErrorCode
                        };
                    }

                    return Export(metaModel, userCurrencyList, totalRow);
                }

                ticketResult = _service.GetBetListForMultipleCustomers(metaModel, userCurrencyList);
            }

            return GenerateHtmlViewModel(metaModel, ticketResult, isAllowedCustomerComm);
        }

        private dynamic Export(MultipleCustomersBetListMetaModel metaModel, IDictionary<int, string> userCurrencyList, int totalRow)
        {
            var title = _service.GetTitle(metaModel);

            if (totalRow <= MaxRowPerFile)
            {
                var normalTicketResult = _service.GetBetListForMultipleCustomers(metaModel, userCurrencyList);
                if (normalTicketResult.MainTicketList.Count == 0)
                {
                    return new HttpResponseMessage(HttpStatusCode.NoContent);
                }

                return ExportXls(normalTicketResult, title);
            }

            var ticketResult = _service.GetBetListForMultipleCustomers(metaModel, MaxRowPerFile, userCurrencyList);


            return ExportZip(ticketResult, title);
        }

        private dynamic GenerateHtmlViewModel(MultipleCustomersBetListMetaModel metaModel, TicketResult ticketResult, bool isAllowedCustomerComm)
        {
            return new
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
        }


        private HttpResponseMessage ExportXls(TicketResult ticketResult, string title)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                ShowTotal = true,
                ShowSumStake = true,
                ShowLicenseeWl = true,
                ShowWinLoss = true,
                CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                Title = title
            };

            var excel = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
            var workbook = excel.Export(excelModel);
            return ExcelStream(workbook, string.Empty);
        }
        private BettingTicketCenterExcelModel CreateBettingTicketCenterExcelModel(GroupTicketResult groupData, string dataPath, string title)
        {
            var contentData = _tempFileService.ReadString(dataPath);
            if (!string.IsNullOrEmpty(contentData))
            {
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
                    CustomerCommVisible = SessionService.LicenseeSession.IsAllowedCustomerComm,
                    Title = title
                };
            }
            return null;
        }

        private HttpResponseMessage ExportZip(GroupTicketResult ticketResult, string title)
        {
            IWorkbook workbook = null;

            using (var zip = new ZipFile())
            {
                for (int i = 0; i < ticketResult.DataPath.Count; i++)
                {

                    var excelService = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();
                    var excelModel = CreateBettingTicketCenterExcelModel(ticketResult, ticketResult.DataPath[i], title);
                    if (excelModel != null)
                    {
                        workbook = excelService.Export(excelModel);
                        using (var excelStream = new MemoryStream())
                        {
                            workbook.Write(excelStream);
                            zip.AddEntry($"sheet{ i}.xls", excelStream.ToArray());
                            workbook.Clear();
                            excelModel.Data.MainTicketList.TrimExcess();
                            excelModel.Data = null;
                        }
                    }

                    _tempFileService.Delete(ticketResult.DataPath[i]);

                }
                return ZipContentResult(zip);
            }
        }

        protected HttpResponseMessage ZipContentResult(ZipFile zipFile)
        {
            var pushStreamContent = new PushStreamContent(
                (stream, content, context) =>
                {
                    zipFile.Save(stream);
                    stream.Close();
                },
                "application/zip");

            return new HttpResponseMessage(HttpStatusCode.OK) { Content = pushStreamContent };
        }
    }
}