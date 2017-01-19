namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class TodayWithdrawalController : ApiController
    {
        private readonly ISimpleWidgetService _todayWithdrawalService;

        public TodayWithdrawalController() : this(null)
        {
        }

        public TodayWithdrawalController(ISimpleWidgetService todayWithdrawal)
        {
            _todayWithdrawalService = todayWithdrawal ?? new TodayWithdrawalService();
        }

        [HttpPost]
        public SimpleWidgetViewModel Index(DashboardBaseModel metaModel)
        {
            return _todayWithdrawalService.GetData(metaModel);
        }
    }
}