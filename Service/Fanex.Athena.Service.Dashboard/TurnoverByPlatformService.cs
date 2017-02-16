namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TurnoverByPlatformService : IWidgetByPlatformService
    {
        private readonly IWidgetByPlatformDataAccess _turnoverByPlatformDataAccess;

        public TurnoverByPlatformService() : this(null)
        {
        }

        public TurnoverByPlatformService(IWidgetByPlatformDataAccess turnoverByPlatformDataAccess)
        {
            _turnoverByPlatformDataAccess = turnoverByPlatformDataAccess ?? new TurnoverByPlatformDataAccess();
        }

        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            return _turnoverByPlatformDataAccess.GetData(metaModel);
        }
    }
}