using System.Collections.Generic;
using System.Data;
using Fanex.Athena.DataAccess.Core;
using Fanex.Athena.DataAccess.Customer.Interfaces;

namespace Fanex.Athena.DataAccess.Customer
{
    public class SearchCustomerDataAccess : BaseDataAccess, ISearchCustomerDataAccess
    {
        public IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml, bool isIncludeDormant)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerSelUserSearch"))
            {
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("username", userName);
                db.SetParameterValue("AgentList", agentListXml);
                db.SetParameterValue("IsIncludeDormant", isIncludeDormant);
                return db.Query<string>();
            }
        }
    }
}