namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
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

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return _todayWithdrawalService.GetData(metaModel);
        }
    }
}