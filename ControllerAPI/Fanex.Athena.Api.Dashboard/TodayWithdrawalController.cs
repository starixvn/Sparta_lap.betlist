namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class TodayWithdrawalController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public TodayWithdrawalController(ISingleValueDataAccess todayWithdrawal)
        {
            _dataAccess = todayWithdrawal;
        }

        public async Task<decimal> GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return await _dataAccess.GetTodayWithdrawal(metaModel).ConfigureAwait(false);
        }
    }
}