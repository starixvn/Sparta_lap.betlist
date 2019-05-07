using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.DataAccess.Customer.Interfaces
{
    public interface ISearchCustomerDataAccess
    {
        Task<IEnumerable<string>> SearchCustomers(int userId, string userName, string agentListXml, bool isIncludeDormant);
    }
}