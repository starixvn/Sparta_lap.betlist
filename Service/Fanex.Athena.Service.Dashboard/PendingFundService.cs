namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    public class PendingFundService : ISimpleWidgetService
    {
        private readonly ISimpleWidgetDataAccess _pendingFundDataAccess;

        public PendingFundService() : this(null)
        {
        }

        public PendingFundService(ISimpleWidgetDataAccess pendingFundDataAccess)
        {
            _pendingFundDataAccess = pendingFundDataAccess ?? new PendingFundDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _pendingFundDataAccess.GetData(metaModel);
        }
    }
}