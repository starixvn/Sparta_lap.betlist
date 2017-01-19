namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TodayDepositService : ITodayDepositService
    {
        private readonly ITodayDepositDataAccess _todayDepositDataAccess;

        public TodayDepositService()
        {
        }

        public TodayDepositService(ITodayDepositDataAccess todayDepositDataAccess)
        {
            _todayDepositDataAccess = todayDepositDataAccess ?? new TodayDepositDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _todayDepositDataAccess.GetData(metaModel);
        }
    }
}