using System.Web.Mvc;
using Fanex.Athena.Model.BetList.MetaModel;
using Fanex.Athena.Service.BetList.Interfaces;
using Fanex.BetList.Core.Builder;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.Controller
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
        public string Detail(SystemParlayDetailMetaModel metaModel)
        {
            var systemParlayData = _betListService.GetSystemParlayDetail(metaModel);

            return _betListBuilder.BuildSystemParlayDetail(this._ticketHelper, systemParlayData);
        }
    }
}