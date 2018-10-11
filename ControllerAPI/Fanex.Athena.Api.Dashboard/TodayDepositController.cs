namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class TodayDepositController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public TodayDepositController(ISingleValueDataAccess dataAccess)
        {
            this._dataAccess = dataAccess;
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return _dataAccess.GetTodayDeposit(metaModel);
        }
    }
}