namespace Fanex.Athena.DataAccess.Customer
{
    using System.Data;
    using Core;
    using Interfaces;
    using Model.Customer.DTO;

    public class CustomerDataAccess : BaseDataAccess, ICustomerDataAccess
    {
        public CustomerInfo GetCustomerInfo(int custId)
        {
            const string UspName = "Lrf_CustomerSelStatus";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);

                return db.QueryEntity<CustomerInfo>();
            }
        }
    }
}