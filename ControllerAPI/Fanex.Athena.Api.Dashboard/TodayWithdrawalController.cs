namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class TodayWithdrawalController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetService _todayWithdrawalService;

        public TodayWithdrawalController() : this(null)
        {
        }

        public TodayWithdrawalController(ISimpleWidgetService todayWithdrawal)
        {
            _todayWithdrawalService = todayWithdrawal ?? new TodayWithdrawalService();
        }

        public SimpleWidgetViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            return _todayWithdrawalService.GetData(metaModel);
        }
    }
}