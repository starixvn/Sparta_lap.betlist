namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class PendingFundController : ApiController
    {
        private readonly ISimpleWidgetService _pendingFundService;

        public PendingFundController() : this(null)
        {
        }

        public PendingFundController(ISimpleWidgetService pendingFundService)
        {
            _pendingFundService = pendingFundService ?? new PendingFundService();
        }

        [HttpPost]
        public SimpleWidgetViewModel Index(DashboardBaseModel metaModel)
        {
            return _pendingFundService.GetData(metaModel);
        }
    }
}