namespace Fanex.Athena.Service.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    public class SportsbookWalletService : ISportsbookWalletService
    {
        private readonly ISportsbookWalletDataAccess _sportsbookWalletDataAccess;

        public SportsbookWalletService() : this(null)
        {
        }

        public SportsbookWalletService(ISportsbookWalletDataAccess sportsbookWalletDataAccess)
        {
            _sportsbookWalletDataAccess = sportsbookWalletDataAccess ?? new SportsbookWalletDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _sportsbookWalletDataAccess.GetData(metaModel);
        }
    }
}