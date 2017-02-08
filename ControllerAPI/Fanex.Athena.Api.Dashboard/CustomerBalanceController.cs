namespace Fanex.Athena.Api.Dashboard
{
    using System;
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class CustomerBalanceController : BaseDashboardApiController
    {
        private readonly ISimpleWidgetService _customerBalanceService;

        public CustomerBalanceController() : this(null)
        {
        }

        public CustomerBalanceController(ISimpleWidgetService customerBalanceService)
        {
            _customerBalanceService = customerBalanceService ?? new CustomerBalanceService();
        }

        public decimal GetData([FromUri] DashboardBaseModel metaModel)
        {
            AssignAuthProperties(metaModel);
            metaModel.FromDate = DateTime.Now.Date.AddMonths(-1);
            return _customerBalanceService.GetData(metaModel);
        }
    }
}