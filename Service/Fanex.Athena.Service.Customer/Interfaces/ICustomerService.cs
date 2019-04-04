namespace Fanex.Athena.Service.Customer.Interfaces
{
    using Fanex.Athena.Model.Customer.DTO;

    public interface ICustomerService
    {
        CustomerInfo GetCustomerInfo(int licenseeId, int custId);
    }
}