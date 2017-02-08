namespace Fanex.Athena.Service.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Athena.Model.Dashboard.ViewModel;
    using Fanex.Athena.Service.Dashboard.Interfaces;

    public class CustomerBalanceService : ISimpleWidgetService
    {
        private readonly ISimpleWidgetDataAccess _customerBalanceDataAccess;

        public CustomerBalanceService() : this(null)
        {
        }

        public CustomerBalanceService(ISimpleWidgetDataAccess customerBalanceDataAccess)
        {
            _customerBalanceDataAccess = customerBalanceDataAccess ?? new CustomerBalanceDataAccess();
        }

        public SimpleWidgetViewModel GetData(DashboardBaseModel metaModel)
        {
            return _customerBalanceDataAccess.GetData(metaModel);
        }
    }
}