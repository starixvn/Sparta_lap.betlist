using System.Threading.Tasks;
using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.Controller
{
    public class SystemParlayBetListController : BaseController
    {
        private readonly ITicketHelper _ticketHelper;
        private readonly IBetListHTMLBuilder _betListBuilder;
        private readonly IBetListService _betListService;

        public SystemParlayBetListController(
            ITicketHelper ticketHelper,
            IBetListHTMLBuilder betListBuilder,
            IBetListService betListService)
        {
            _ticketHelper = ticketHelper;
            _betListBuilder = betListBuilder;
            _betListService = betListService;
        }

        [HttpPost]
        public async Task<string> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var systemParlayData = await _betListService.GetSystemParlayDetail(metaModel).ConfigureAwait(false);

            return _betListBuilder.BuildSystemParlayDetail(this._ticketHelper, systemParlayData);
        }
    }
}