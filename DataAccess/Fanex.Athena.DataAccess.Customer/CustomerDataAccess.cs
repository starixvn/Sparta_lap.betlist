namespace Fanex.Athena.DataAccess.Customer
{
    using System.Data;
    using Core;
    using Core.Exception;
    using Data;
    using Interfaces;
    using Model.Customer.DTO;

    public class CustomerDataAccess : BaseDataAccess, ICustomerDataAccess
    {
        public CustomerInfo GetCustomerInfo(int custId)
        {
            const string UspName = "Lrf_CustomerSelStatus";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);

                    return db.QueryEntity<CustomerInfo>();
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }
    }
}