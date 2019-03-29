namespace Fanex.Athena.Service.Customer.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Customer.DTO;

    public interface ICustomerService
    {
        Task<CustomerInfo> GetCustomerInfo(int licenseeId, int custId);
    }
}