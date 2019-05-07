namespace Fanex.Athena.Module.Dashboard.Api
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Fanex.Athena.Module.Dashboard.Data.Interfaces;
    using Fanex.Athena.Module.Dashboard.MetaModel;

    public class TodayDepositController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public TodayDepositController(ISingleValueDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
        }

        public async Task<decimal> GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return await _dataAccess.GetTodayDeposit(metaModel).ConfigureAwait(false);
        }
    }
}