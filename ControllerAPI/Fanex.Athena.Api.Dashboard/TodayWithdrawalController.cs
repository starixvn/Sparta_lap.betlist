namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class TodayWithdrawalController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetDataAccess _dataAccess;

        public TodayWithdrawalController() : this(null)
        {
        }

        public TodayWithdrawalController(ISimpleWidgetDataAccess todayWithdrawal)
        {
            _dataAccess = todayWithdrawal ?? new TodayWithdrawalDataAccess();
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return _dataAccess.GetData(metaModel);
        }
    }
}