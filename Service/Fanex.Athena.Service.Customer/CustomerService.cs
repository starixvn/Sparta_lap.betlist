namespace Fanex.Athena.Service.Customer
{
    using DataAccess.Customer;
    using Fanex.Athena.DataAccess.Customer.Interfaces;
    using Interfaces;
    using Model.Customer.DTO;
    using Service.Core.Users;
    using Service.Core.Users.Interfaces;

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerDataAccess _customerDataAccess;

        private readonly IAuthorizationService _authorizationService;

        public CustomerService()
        {
            _customerDataAccess = new CustomerDataAccess();
            _authorizationService = new AuthorizationService();
        }

        public CustomerService(ICustomerDataAccess customerDataAccess, IAuthorizationService authorizationService)
        {
            _customerDataAccess = customerDataAccess;
            _authorizationService = authorizationService;
        }

        public CustomerInfo GetCustomerInfo(int licenseeId, int custId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, 0)
                ? _customerDataAccess.GetCustomerInfo(custId)
                : new CustomerInfo();
        }
    }
}