namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class PendingFundController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetService _pendingFundService;

        public PendingFundController() : this(null)
        {
        }

        public PendingFundController(ISimpleWidgetService pendingFundService)
        {
            _pendingFundService = pendingFundService ?? new PendingFundService();
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            return _pendingFundService.GetData(metaModel);
        }
    }
}