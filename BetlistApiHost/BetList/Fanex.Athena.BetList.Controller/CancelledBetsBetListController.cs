using System.Web.Mvc;
using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Controller
{
    public class CancelledBetsBetListController : BaseController
    {
        private readonly ICancelledBetListDataAccess _cancelledBetListDataAccess;

        public CancelledBetsBetListController(ICancelledBetListDataAccess cancelledBetListDataAccess)
        {
            _cancelledBetListDataAccess = cancelledBetListDataAccess;
        }

        [HttpPost]
        public JsonResult GetCustomerList(BetListMetaModel metaModel)
        {
            var result = _cancelledBetListDataAccess.GetCancelBetCustList(metaModel.AgentListXml);
            return Json(result);
        }
    }
}