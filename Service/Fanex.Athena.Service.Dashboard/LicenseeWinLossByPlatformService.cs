namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class LicenseeWinLossByPlatformService : IWidgetByPlatformService
    {
        private readonly IWidgetByPlatformDataAccess _licenseeWinLossByPlatformDataAccess;

        public LicenseeWinLossByPlatformService() : this(null)
        {
        }

        public LicenseeWinLossByPlatformService(IWidgetByPlatformDataAccess licenseeWinLossByPlatformDataAccess)
        {
            _licenseeWinLossByPlatformDataAccess = licenseeWinLossByPlatformDataAccess ?? new LicenseeWinLossByPlatformDataAccess();
        }

        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            return _licenseeWinLossByPlatformDataAccess.GetData(metaModel);
        }
    }
}