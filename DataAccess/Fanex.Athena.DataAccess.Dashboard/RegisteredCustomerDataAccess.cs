namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class RegisteredCustomerDataAccess : IRegisteredCustomerDataAccess
    {
        public RegisteredCustomerViewModel GetData(DashboardBaseModel metaModel)
        {
            return new RegisteredCustomerViewModel { DepositCount = 400, NonDepositCount = 100 };
        }
    }
}
