namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TurnoverByPlatformService : ITurnoverByPlatformService
    {
        private readonly ITurnoverByPlatformDataAccess _turnoverByPlatformDataAccess;

        public TurnoverByPlatformService() : this(null)
        {
        }

        public TurnoverByPlatformService(ITurnoverByPlatformDataAccess turnoverByPlatformDataAccess)
        {
            _turnoverByPlatformDataAccess = turnoverByPlatformDataAccess ?? new TurnoverByPlatformDataAccess();
        }

        public TurnoverByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            return new TurnoverByPlatformViewModel
            {
                Data = _turnoverByPlatformDataAccess.GetData(metaModel)
            };
        }
    }
}