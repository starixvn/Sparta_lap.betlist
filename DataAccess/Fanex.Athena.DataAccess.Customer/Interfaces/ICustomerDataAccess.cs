namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    using Fanex.Athena.Model.Customer.DTO;

    public interface ICustomerDataAccess
    {
        CustomerInfo GetCustomerInfo(int custId);
    }
}