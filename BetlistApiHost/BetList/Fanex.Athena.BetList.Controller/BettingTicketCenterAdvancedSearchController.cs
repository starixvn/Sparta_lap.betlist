namespace Fanex.Athena.BetList.Controller
{
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.BetList.Controller.CustomActionResult;
    using Fanex.Athena.BetList.Controller.Utility;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.Athena.Resources;
    using Newtonsoft.Json;

    public class BettingTicketCenterAdvancedSearchController : BettingTicketCenterBaseController
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private const int ExportMultipleFile = 1002;

        private const int ExportSingleFile = 1003;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private readonly IBettingTicketCenterZipBuilder _zipBuilder;

        private readonly IIPService _ipService;

        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterAdvancedSearchController(
            IAppSettings appSettings,
            IBettingTicketCenterAdvancedSearchDataAccess dataAccess,
            IIPService ipService,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterZipBuilder zipBuilder,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            ITempFileService tempFileService) : base(ticketBuilderService, bettingTicketCenterExcel)
        {
            _appSettings = appSettings;
            _dataAccess = dataAccess;
            _ipService = ipService;
            _zipBuilder = zipBuilder ?? new BettingTicketCenterZipBuilder(_appSettings, null, bettingTicketCenterExcel, ipService);
            _tempFileService = tempFileService;
        }

        private int MaxTicketLimit => int.Parse(_appSettings.GetValue("MaxTicketLimit"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        private int MaxRowForMultipleExcelFiles => int.Parse(_appSettings.GetValue("MaxRowForMultipleExcelFiles"));

        private int MaxRowSupportSingleExportExcel => int.Parse(_appSettings.GetValue("MaxRowSupportSingleExportExcel"));

        [HttpPost]
        public async Task<ActionResult> GetExportStatus(AdvancedSearchBetListMetaModel metaModel)
        {
            var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel).ConfigureAwait(false);

            if (totalRow > MaxTicketLimit)
            {
                return Json(new
                {
                    StatusCode = RowsExceedLimitErrorCode
                });
            }

            if (totalRow <= MaxRowSupportSingleExportExcel)
            {
                return Json(new
                {
                    StatusCode = ExportSingleFile
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
                return await Export(metaModel).ConfigureAwait(false);
            }

            var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel).ConfigureAwait(false);

            if (totalRow > MaxTicketLimit)
            {
                return Json(new
                {
                    StatusCode = RowsExceedLimitErrorCode
                });
            }

            var ticketResult = ShouldReturnEmptyResult(metaModel)
                ? new TicketResult()
                : await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel
                ).ConfigureAwait(false);

            return ReturnData(ticketResult, GetBetListOption(metaModel), metaModel);
        }

        private async Task<ActionResult> Export(AdvancedSearchBetListMetaModel metaModel)
        {
            var title = GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}";

            var ticketResult = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, MaxRowPerFile
                ).ConfigureAwait(false);

            if (!ticketResult.DataPath.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            if (ticketResult.TotalRows <= MaxRowPerFile)
            {
                if (ticketResult.TotalRows == 0 || ShouldReturnEmptyResult(metaModel))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }

                var tickets = GetTicketFromFile(ticketResult);
                return ExportXls(tickets, title, metaModel, totalText);
            }

            if (ShouldExportNextPage(metaModel.PageSize, ticketResult.CurrentPageRowCount))
            {
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
            }

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

        protected FileContentResult ZipContentResult(MemoryStream stream, AdvancedSearchBetListMetaModel metaModel)
        {
            var bytes = stream?.ToArray();
            Response.Headers.Add("X-File-Name", $"BettingTicketsCenter_AdvancedSearch{metaModel.PageIndex}");
            return File(bytes, "application/zip", "BettingTicketsCenter_AdvancedSearch.zip");
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
            SpecialPlatform = metaModel.SpecialPlatform,
        };

        private bool ShouldReturnEmptyResult(AdvancedSearchBetListMetaModel metaModel)
            => !string.IsNullOrWhiteSpace(metaModel.Ip) && !metaModel.IsShowIpvn && _ipService.IsVNIP(metaModel.Ip);

        private bool ShouldExportNextPage(int pageSize, int currentPageRowCount)
            => pageSize == MaxRowForMultipleExcelFiles
                && currentPageRowCount == MaxRowForMultipleExcelFiles;

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