namespace Fanex.Athena.Service.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    public class SportsbookWalletService : ISimpleWidgetService
    {
        private readonly ISimpleWidgetDataAccess _sportsbookWalletDataAccess;

        public SportsbookWalletService() : this(null)
        {
        }

        public SportsbookWalletService(ISimpleWidgetDataAccess sportsbookWalletDataAccess)
        {
            _sportsbookWalletDataAccess = sportsbookWalletDataAccess ?? new SportsbookWalletDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _sportsbookWalletDataAccess.GetData(metaModel);
        }
    }
}