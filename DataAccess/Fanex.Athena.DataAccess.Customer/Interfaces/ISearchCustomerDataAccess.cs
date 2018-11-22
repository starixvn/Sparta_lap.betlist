using System.Collections.Generic;

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    public interface ISearchCustomerDataAccess
    {
        IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml, bool isIncludeDormant);
    }
}