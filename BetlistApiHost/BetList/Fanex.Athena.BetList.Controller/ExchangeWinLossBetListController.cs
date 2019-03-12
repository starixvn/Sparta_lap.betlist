using System.Web.Mvc;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.Athena.BetList.Service.Interfaces;

namespace Fanex.Athena.BetList.Controller
{
    public class ExchangeWinLossBetListController : BaseController
    {
        private readonly IExchangeBetListService _service;

        public ExchangeWinLossBetListController(IExchangeBetListService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResult GetData(ExchangeBetListMetaModel metaModel)
        {
            return Json(_service.GetBetListData(metaModel));
        }

        [HttpPost]
        public JsonResult Detail(ExchangeBetListDetailMetaModel metaModel)
        {
            return Json(_service.GetTicketDetailList(metaModel));
        }
    }
}