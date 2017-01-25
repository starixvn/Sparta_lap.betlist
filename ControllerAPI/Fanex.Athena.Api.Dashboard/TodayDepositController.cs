namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class TodayDepositController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetService _todayDepositService;

        public TodayDepositController() : this(null)
        {
        }

        public TodayDepositController(ISimpleWidgetService todayDepositService)
        {
            this._todayDepositService = todayDepositService ?? new TodayDepositService();
        }

        public SimpleWidgetViewModel GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            return _todayDepositService.GetData(metaModel);
        }
    }
}