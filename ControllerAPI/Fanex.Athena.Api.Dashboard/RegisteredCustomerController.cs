namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using Service.Dashboard;
    using Service.Dashboard.Interfaces;

    public class RegisteredCustomerController : BaseDashboardApiController
    {
        private readonly IRegisteredCustomerService _registeredCustomerService;

        public RegisteredCustomerController() : this(null)
        {
        }

        public RegisteredCustomerController(IRegisteredCustomerService registeredCustomerService)
        {
            _registeredCustomerService = registeredCustomerService ?? new RegisteredCustomerService();
        }

        public RegisteredCustomerViewModel Index(DashboardBaseModel metaModel)
        {
            return _registeredCustomerService.GetData(metaModel);
        }
    }
}
