using System.Threading.Tasks;
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
        public async Task<JsonResult> GetCustomerList(string agentListXml)
        {
            var result = await _cancelledBetListDataAccess.GetCancelBetCustList(agentListXml);
            return Json(result);
        }
    }
}