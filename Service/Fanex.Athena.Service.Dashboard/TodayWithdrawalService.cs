namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TodayWithdrawalService : ISimpleWidgetService
    {
        private readonly ISimpleWidgetDataAccess _todayWithdrawalDataAccess;

        public TodayWithdrawalService() : this(null)
        {
        }

        public TodayWithdrawalService(ISimpleWidgetDataAccess todayWithdrawalDataAccess)
        {
            _todayWithdrawalDataAccess = todayWithdrawalDataAccess ?? new TodayWithdrawalDataAccess();
        }

        public decimal GetData(DashboardBaseModel metaModel)
        {
            return _todayWithdrawalDataAccess.GetData(metaModel);
        }
    }
}