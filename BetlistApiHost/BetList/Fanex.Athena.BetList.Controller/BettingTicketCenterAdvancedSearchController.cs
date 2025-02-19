namespace Fanex.Athena.BetList.Controller
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using CustomActionResult;
    using DataAccess.Interfaces;
    using Excel;
    using Excel.Model;
    using Model.DTO;
    using Model.MetaModel;
    using Newtonsoft.Json;
    using Resources;
    using Service.Interfaces;
    using Utility;

    public class BettingTicketCenterAdvancedSearchController : BettingTicketCenterBaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private const int ExportMultipleFile = 1002;

        private const int ExportSingleFile = 1003;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private readonly IIPService _ipService;

        private readonly ILogAggregateService _logAggregateService;

        private readonly ITempFileService _tempFileService;

        private readonly IBettingTicketCenterZipBuilder _zipBuilder;

        public BettingTicketCenterAdvancedSearchController(
            IAppSettings appSettings,
            IBettingTicketCenterAdvancedSearchDataAccess dataAccess,
            IIPService ipService,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterZipBuilder zipBuilder,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            ITempFileService tempFileService,
            ILogAggregateService logAggregateService) : base(ticketBuilderService, bettingTicketCenterExcel)
        {
            _appSettings = appSettings;
            _dataAccess = dataAccess;
            _ipService = ipService;
            _zipBuilder = zipBuilder ?? new BettingTicketCenterZipBuilder(_appSettings, null, bettingTicketCenterExcel, ipService, _logAggregateService);
            _tempFileService = tempFileService;
            _logAggregateService = logAggregateService;
        }

        private int MaxTicketLimit => int.Parse(_appSettings.GetValue("MaxTicketLimit"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        private int MaxRowForMultipleExcelFiles => int.Parse(_appSettings.GetValue("MaxRowForMultipleExcelFiles"));

        private int MaxRowSupportSingleExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportSingleExportExcel"));

        [HttpPost]
        public async Task<ActionResult> GetExportStatus(AdvancedSearchBetListMetaModel metaModel)
        {
            var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, MaxTicketLimit).ConfigureAwait(false);

            if (totalRow > MaxTicketLimit)
            {
                return Json(new
                {
                    StatusCode = RowsExceedLimitErrorCode,
                    LimitRows = MaxTicketLimit
                });
            }

            if (totalRow <= MaxRowSupportSingleExportExcel)
            {
                return Json(new
                {
                    StatusCode = ExportSingleFile,
                    PageSize = MaxRowSupportSingleExportExcel
                });
            }

            return Json(new
            {
                StatusCode = ExportMultipleFile,
                PageSize = MaxRowForMultipleExcelFiles
            });
        }

        [HttpPost]
        public async Task<ActionResult> GetData(AdvancedSearchBetListMetaModel metaModel)
        {
            if (metaModel.IsExport)
            {
                _logAggregateService.Append("Start request");
                var result = await Export(metaModel).ConfigureAwait(false);
                _logAggregateService.Append("End request");
                _logAggregateService.SendLog();

                return result;
            }

            var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, MaxTicketLimit).ConfigureAwait(false);

            if (totalRow > MaxTicketLimit)
            {
                return Json(new
                {
                    StatusCode = RowsExceedLimitErrorCode,
                    LimitRows = MaxTicketLimit
                });
            }

            var ticketResult = ShouldReturnEmptyResult(metaModel)
                ? new TicketResult()
                : await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel
                ).ConfigureAwait(false);

            return ReturnData(ticketResult, GetBetListOption(metaModel), metaModel);
        }

        protected FileContentResult ZipContentResult(MemoryStream stream, AdvancedSearchBetListMetaModel metaModel)
        {
            var bytes = stream?.ToArray();
            Response.Headers.Add("X-File-Name", $"BettingTicketsCenter_AdvancedSearch{metaModel.PageIndex}");
            return File(bytes, "application/zip", "BettingTicketsCenter_AdvancedSearch.zip");
        }

        private async Task<ActionResult> Export(AdvancedSearchBetListMetaModel metaModel)
        {
            var title = GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}";

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(
                metaModel,
                metaModel.PageSize == MaxRowSupportSingleExportExcel ? MaxRowSupportSingleExportExcel : MaxRowPerFile
            ).ConfigureAwait(false);
            _logAggregateService.Append("Done get data");

            if (!ticketResult.DataPath.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            if (ticketResult.TotalRows <= MaxRowSupportSingleExportExcel)
            {
                if (ticketResult.TotalRows == 0 || ShouldReturnEmptyResult(metaModel))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }

                _logAggregateService.Append("Export single excel");

                var tickets = GetTicketFromFile(ticketResult);
                return ExportXls(tickets, title, metaModel, totalText);
            }

            if (ShouldExportNextPage(metaModel.PageSize, ticketResult.CurrentPageRowCount))
            {
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
            }

            _logAggregateService.Append("Export zip excel");

            var zipFile = _zipBuilder.BuildZip(HttpContext.Response,
                ticketResult, title, metaModel, totalText);
            return zipFile == null ? null : ZipContentResult(zipFile, metaModel);
        }

        private ExcelResult ExportXls(TicketResult ticketResult, string title, AdvancedSearchBetListMetaModel metaModel, string totalText)
        {
            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                Option = GetBetListOption(metaModel),
                Title = title,
                TotalText = totalText
            };

            return ExportExcel(excelModel);
        }

        private static BettingTicketCenterOption GetBetListOption(AdvancedSearchBetListMetaModel metaModel) => new BettingTicketCenterOption
        {
            ShowTotal = true,
            ShowSumStake = true,
            ShowLicenseeWl = true,
            ShowWinLoss = true,
            ShowExternalId = true,
            ShowSystemId = true,
            IsAllowedCustomerComm = metaModel.HasPlayerCom,
            ShowTxId = metaModel.IsAllowTxIDColumn,
            IsShowIpvn = metaModel.IsShowIpvn,
            IsShowIp = metaModel.IsShowIp,
            SpecialPlatform = metaModel.SpecialPlatform
        };

        private bool ShouldReturnEmptyResult(AdvancedSearchBetListMetaModel metaModel)
            => !string.IsNullOrWhiteSpace(metaModel.Ip) && !metaModel.IsShowIpvn && _ipService.IsVNIP(metaModel.Ip);

        private static bool ShouldExportNextPage(int pageSize, int currentPageRowCount)
            => pageSize == currentPageRowCount;

        private TicketResult GetTicketFromFile(GroupTicketResult groupTicket)
        {
            var contentData = _tempFileService.ReadString(groupTicket.DataPath[0]);
            _tempFileService.Delete(groupTicket.DataPath[0]);
            var ticketResult = JsonConvert.DeserializeObject<TicketResult>(contentData);

            ticketResult.SumStake = groupTicket.SumStake;
            ticketResult.SumWinLoss = groupTicket.SumWinLoss;
            ticketResult.SumLicenseeWL = groupTicket.SumLicenseeWL;
            ticketResult.SumComm = groupTicket.SumComm;
            ticketResult.SumPlayerComm = groupTicket.SumPlayerComm;
            ticketResult.TotalRows = groupTicket.TotalRows;
            return ticketResult;
        }
    }
}