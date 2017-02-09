namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
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

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return _todayDepositService.GetData(metaModel);
        }
    }
}