using System.Web.Mvc;
using Fanex.Athena.DataAccess.BetList.Interfaces;
using Fanex.Athena.Model.BetList.MetaModel;

namespace Fanex.Athena.Controller
{
    public class OutstandingDailyStatementBetListController : BaseController
    {
        private readonly IDailyStatementBetListDataAccess _dailyStatementBetListDataAccess;

        public OutstandingDailyStatementBetListController(IDailyStatementBetListDataAccess dailyStatementBetListDataAccess)
        {
            _dailyStatementBetListDataAccess = dailyStatementBetListDataAccess;
        }

        [HttpPost]
        public JsonResult GetData(BetListMetaModel metaModel)
        {
            var dataList = _dailyStatementBetListDataAccess.GetDailyStatementData(metaModel);
            return Json(new
            {
                Main = dataList,
                Total = new
                {
                    SumWinLoss = 0,
                    SumLicenseeWl = 0
                }
            });
        }
    }
}