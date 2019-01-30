using System.Web.Mvc;
using Fanex.Athena.DataAccess.BetList.Interfaces;
using Fanex.Athena.Model.BetList.MetaModel;

namespace Fanex.Athena.Controller
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