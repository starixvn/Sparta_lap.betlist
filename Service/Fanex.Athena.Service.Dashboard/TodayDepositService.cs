namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TodayDepositService : ISimpleWidgetService
    {
        private readonly ISimpleWidgetDataAccess _todayDepositDataAccess;

        public TodayDepositService()
        {
        }

        public TodayDepositService(ISimpleWidgetDataAccess todayDepositDataAccess)
        {
            _todayDepositDataAccess = todayDepositDataAccess ?? new TodayDepositDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _todayDepositDataAccess.GetData(metaModel);
        }
    }
}