namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;

    public class CustomerBalanceController : BaseDashboardApiController
    {
        private readonly ISingleValueDataAccess _dataAccess;

        public CustomerBalanceController(ISingleValueDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            if (!metaModel.IsValid)
            {
                return 0;
            }

            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date;
            return _dataAccess.GetCustomerBalance(metaModel);
        }
    }
}