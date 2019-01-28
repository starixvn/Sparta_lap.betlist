using System.Collections.Generic;
using System.Web.Http;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Model.BetList.MetaModel;
using Fanex.Athena.Model.BetList.ViewModel;
using Fanex.Athena.Service.BetList.Interfaces;

namespace Fanex.Athena.Controller
{
    public class ExchangeWinLossBetListController : BaseController
    {
        private readonly IExchangeBetListService _service;

        public ExchangeWinLossBetListController(IExchangeBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public ExchangeBetListViewModel GetData(ExchangeBetListMetaModel metaModel)
        {
            return _service.GetBetListData(metaModel);
        }

        [HttpPost]
        public IEnumerable<ExchangeTicketDetail> Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            return _service.GetTicketDetailList(metaModel);
        }
    }
}