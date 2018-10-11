namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class PendingFundController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public PendingFundController(ISingleValueDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            return _dataAccess.GetPendingFunds(metaModel);
        }
    }
}