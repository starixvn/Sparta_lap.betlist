namespace Fanex.Athena.Service.Dashboard
{
    using DataAccess.Dashboard;
    using DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Service.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class RegisteredCustomerService : IRegisteredCustomerService
    {
        private IRegisteredCustomerDataAccess _registeredCustomerDataAccess;

        public RegisteredCustomerService() : this(null)
        {
        }

        public RegisteredCustomerService(IRegisteredCustomerDataAccess registeredCustomerDataAccess)
        {
            _registeredCustomerDataAccess = registeredCustomerDataAccess ?? new RegisteredCustomerDataAccess();
        }

        public RegisteredCustomerViewModel GetData(DashboardBaseModel metaModel)
        {
            return _registeredCustomerDataAccess.GetData(metaModel);
        }
    }
}