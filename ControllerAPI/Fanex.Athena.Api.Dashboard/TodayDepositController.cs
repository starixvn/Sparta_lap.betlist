namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class TodayDepositController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetDataAccess _dataAccess;

        public TodayDepositController() : this(null)
        {
        }

        public TodayDepositController(ISimpleWidgetDataAccess dataAccess)
        {
            this._dataAccess = dataAccess ?? new TodayDepositDataAccess();
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