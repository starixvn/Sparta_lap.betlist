namespace Fanex.Athena.Api.BetList
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Base;
    using Base.Attributes;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Ionic.Zip;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NPOI.SS.UserModel;
    using Service.BetList.Interfaces;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Service.Core.Utility.Interfaces;

    [ApiFunctionAuthorization(PermissionKeys.BettingTicketCenter)]
    public class BettingTicketCenterMultiCustomerController : BaseApiController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private const int MaxRowSupportExportExcel = 300000;

        private const int MaxRowPerFile = 65;

        private readonly ICoreLogger _logger;

        private readonly IBettingTicketCenterService _service;

        public BettingTicketCenterMultiCustomerController(
            ISessionService sessionService,
            IProductService productService,
            ILanguageService langService,
            ICoreLogger logger,
            IBettingTicketCenterService service)
            : base(sessionService, productService, langService)
        {
            _logger = logger;
            _service = service;
        }

        [System.Web.Http.HttpPost]
        public dynamic GetData(MultipleCustomersBetListMetaModel metaModel)
        {
            TicketResult ticketResult;
            var userCurrencyList = SessionService.UserSession.CurrencyList;
            var isAllowedCustomerComm = SessionService.LicenseeSession.IsAllowedCustomerComm;
            if (!metaModel.IsValid || !_service.IsValidCurrencyId(metaModel.CurrencyId, userCurrencyList))
            {
                ticketResult = new TicketResult();
            }
            else
            {
                metaModel.AgentListXml = SessionService.GenerateAgentListXml(metaModel.CurrencyId);

                if (metaModel.IsExport && _service.GetTotalRowForMultipleCustomers(metaModel) > MaxRowSupportExportExcel)
                {
                    return new
                    {
                        ErrorCode = RowsExceedLimitErrorCode
                    };
                }

                ticketResult = _service.GetBetListForMultipleCustomers(metaModel, userCurrencyList);
            }

            if (metaModel.IsExport)
            {
                return Export(ticketResult, _service.GetTitle(metaModel));
            }

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

        private HttpResponseMessage Export(TicketResult ticketResult, string title)
        {
            if (ticketResult.MainTicketList.Count == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            return ticketResult.MainTicketList.Count > MaxRowPerFile ?
                ExportZip(ticketResult, title) :
                ExportXls(ticketResult, title);
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

        private HttpResponseMessage ExportZip(TicketResult ticketResult, string title)
        {
            IWorkbook workbook = null;
            var dataList = ticketResult.MainTicketList.ToList();
            ticketResult.MainTicketList.Clear();
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

            var numOfFile = (int)Math.Ceiling((double)dataList.Count / MaxRowPerFile);
            using (var zip = new ZipFile())
            {
                for (int i = 0; i < numOfFile; i++)
                {
                    var startIndex = i * MaxRowPerFile;

                    ticketResult.MainTicketList.Clear();
                    ticketResult.MainTicketList.AddRange(dataList.Skip(startIndex).Take(MaxRowPerFile));
                    var excelService = DependencyResolver.Current.GetService<IBettingTicketCenterExcel>();

                    workbook = excelService.Export(excelModel);
                    using (var excelStream = new MemoryStream())
                    {
                        workbook.Write(excelStream);
                        zip.AddEntry($"sheet{ i}.xls", excelStream.ToArray());
                        workbook.Clear();
                    }
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