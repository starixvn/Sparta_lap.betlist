namespace Fanex.Athena.Module.Dashboard.Api
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Fanex.Athena.Module.Dashboard.Data.Interfaces;
    using Fanex.Athena.Module.Dashboard.MetaModel;

    public class PendingFundController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public PendingFundController(ISingleValueDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<decimal> GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            return await _dataAccess.GetPendingFunds(metaModel).ConfigureAwait(false);
        }
    }
}