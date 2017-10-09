namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class PendingFundController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetDataAccess _dataAccess;

        public PendingFundController() : this(null)
        {
        }

        public PendingFundController(ISimpleWidgetDataAccess pendingFundService)
        {
            _dataAccess = pendingFundService ?? new PendingFundDataAccess();
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            return _dataAccess.GetData(metaModel);
        }
    }
}