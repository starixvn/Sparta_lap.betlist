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

        private readonly ISystemParlayDetailService _systemParlayDetailService;

        public SystemParlayBetListController(
            ITicketHelper ticketHelper,
            IBetListHTMLBuilder betListBuilder,
            IBetListService betListService,
            ISystemParlayDetailService systemParlayDetailService)
        {
            _ticketHelper = ticketHelper;
            _betListBuilder = betListBuilder;
            _betListService = betListService;
            _systemParlayDetailService = systemParlayDetailService;
        }

        [HttpPost]
        public async Task<string> Detail(SystemParlayDetailMetaModel metaModel)
        {
            var viewPath = Server.MapPath("~/Views/SystemParlayDetail/Index.cshtml");
            _ticketHelper.ShowSystemParlayPlayerComm = metaModel.IsAllowedCustomerComm;

            return await _systemParlayDetailService.BuildHtmlContent(metaModel, _ticketHelper, viewPath);
        }
    }
}