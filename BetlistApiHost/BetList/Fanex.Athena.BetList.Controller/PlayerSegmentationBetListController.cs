namespace Fanex.Athena.BetList.Controller
{
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class PlayerSegmentationBetListController : BaseController
    {
        private readonly IPlayerSegmentationBetListDataAccess _dataAccess;
        private readonly ITicketBuilderService _ticketBuilderService;

        public PlayerSegmentationBetListController(
            IPlayerSegmentationBetListDataAccess dataAccess,
            ITicketBuilderService ticketBuilderService)
        {
            _dataAccess = dataAccess;
            _ticketBuilderService = ticketBuilderService;
        }

        [HttpPost]
        public async Task<JsonResult> AbnormalBets(PlayerSegmentationBetListMetaModel metaModel)
        {
            var tickets = await _dataAccess.GetAbnormalBetListAsync(metaModel).ConfigureAwait(false);
            var result = new
            {
                Data = _ticketBuilderService.BuildTicketText(tickets, metaModel, metaModel.IsAllowedCustomerComm),
                tickets.TotalRows
            };

            return Json(result);
        }
    }
}