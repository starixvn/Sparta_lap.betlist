namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class TodayDepositController : ApiController
    {
        private readonly ITodayDepositService _todayDepositService;

        public TodayDepositController() : this(null)
        {
        }

        public TodayDepositController(ITodayDepositService todayDepositService)
        {
            this._todayDepositService = todayDepositService ?? new TodayDepositService();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _todayDepositService.GetData(metaModel);
        }
    }
}