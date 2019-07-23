namespace Fanex.Athena.BetList.Controller
{
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Excel.Model;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.Resources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;

    public class PlayerAdminBetListController : BettingTicketCenterBaseController
    {
        private readonly IPlayerAdminBetListDataAccess _dataAccess;

        public PlayerAdminBetListController(
            IPlayerAdminBetListDataAccess dataAccess,
            IBetListHTMLBuilder betlistHtmlBuilder,
            ITicketHelper ticketHelper,
            IBettingTicketCenterExcel excel)
            : base(betlistHtmlBuilder, ticketHelper, excel)
        {
            _dataAccess = dataAccess;
        }

        [HttpPost]
        public async Task<JsonResult> GetBetList(PlayerAdminBetListMetaModel metaModel)
        {
            var ticketResult = await _dataAccess.GetRunningBetListAsync(metaModel).ConfigureAwait(false);

            return ReturnData(ticketResult, GetBetListOption(), metaModel.Currency, 1);
        }

        [HttpPost]
        public async Task<ActionResult> Export(PlayerAdminBetListMetaModel metaModel)
        {
            var ticketResult = await _dataAccess.GetRunningBetListAsync(metaModel).ConfigureAwait(false);

            if (ticketResult.MainTicketList.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

            var excelModel = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                Option = GetBetListOption(),
                TotalText = ViewResources.Total,
                Title = $"{ViewResources.Currency}: {metaModel.Currency}"
            };

            return ExportExcel(excelModel);
        }

        private static BettingTicketCenterOption GetBetListOption() => new BettingTicketCenterOption { ShowTotal = true, ShowSumStake = true };
    }
}
