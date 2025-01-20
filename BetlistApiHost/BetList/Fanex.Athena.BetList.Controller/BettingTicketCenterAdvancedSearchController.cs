namespace Fanex.Athena.BetList.Controller
{
    using Fanex.Athena.BetList.Controller.CustomActionResult;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.Athena.Resources;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class BettingTicketCenterAdvancedSearchController : BettingTicketCenterBaseController
    {
        private const int RowsExceedLimitErrorCode = 209;

        private readonly IAppSettings _appSettings;

        private readonly IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private readonly IIPService _ipService;

        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterAdvancedSearchController(
            IAppSettings appSettings,
            IBettingTicketCenterAdvancedSearchDataAccess dataAccess,
            IIPService ipService,
            ITicketBuilderService ticketBuilderService,
            IBettingTicketCenterExcel bettingTicketCenterExcel,
            ITempFileService tempFileService) : base(ticketBuilderService, bettingTicketCenterExcel)
        {
            _appSettings = appSettings;
            _dataAccess = dataAccess;
            _ipService = ipService;
            _tempFileService = tempFileService;
        }

        private int MaxTicketLimit => int.Parse(_appSettings.GetValue("MaxTicketLimit"));

        private int MaxRowPerFile => int.Parse(_appSettings.GetValue("MaxRowPerExcelFile"));

        [HttpPost]
        public async Task<ActionResult> GetData(AdvancedSearchBetListMetaModel metaModel)
        {
            if (metaModel.IsExport)
            {
                return await Export(metaModel).ConfigureAwait(false);
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

        private async Task<ActionResult> Export(AdvancedSearchBetListMetaModel metaModel)
        {
            if (metaModel.PageIndex == 1)
            {
                var totalRow = await _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, MaxTicketLimit).ConfigureAwait(false);

                if (totalRow == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NoContent);
                }

                if (totalRow > MaxTicketLimit)
                {
                    return new HttpStatusCodeResult(RowsExceedLimitErrorCode, MaxTicketLimit.ToString());
                }
            }

            var title = GetTitle(metaModel);
            var totalText = $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}";

            var groupData = await _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, MaxRowPerFile
                ).ConfigureAwait(false);

            if (!groupData.DataPath.Any() || ShouldReturnEmptyResult(metaModel))
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            if (ShouldExportNextPage(groupData.CurrentPageRowCount))
            {
                Response.StatusCode = (int)HttpStatusCode.PartialContent;
            }

            Response.Headers.Add("X-File-Name",
                $"BettingTicketsCenter_AdvancedSearch{(groupData.TotalRows > MaxRowPerFile ? metaModel.PageIndex.ToString() : string.Empty)}.xlsx");

            var ticketResult = GetTicketFromFile(groupData);

            return ExportXlsx(ticketResult, title, metaModel, totalText);
        }

        private ExcelResult ExportXlsx(TicketResult ticketResult, string title, AdvancedSearchBetListMetaModel metaModel, string totalText)
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
            SpecialPlatform = metaModel.SpecialPlatform,
        };

        private bool ShouldReturnEmptyResult(AdvancedSearchBetListMetaModel metaModel)
            => !string.IsNullOrWhiteSpace(metaModel.Ip) && !metaModel.IsShowIpvn && _ipService.IsVNIP(metaModel.Ip);

        private bool ShouldExportNextPage(int currentPageRowCount)
            => currentPageRowCount == MaxRowPerFile;

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