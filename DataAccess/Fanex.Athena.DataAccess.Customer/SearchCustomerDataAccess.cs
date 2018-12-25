using System.Collections.Generic;
using System.Data;
using Fanex.Athena.DataAccess.Core;
using Fanex.Athena.DataAccess.Customer.Interfaces;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Session.Interfaces;

namespace Fanex.Athena.DataAccess.Customer
{
    public class SearchCustomerDataAccess : BaseDataAccess, ISearchCustomerDataAccess
    {
        public IEnumerable<string> SearchCustomers(int userId, string userName, string agentListXml, bool isIncludeDormant)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerSelUserSearch"))
            {
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("username", DbType.String, userName);
                db.AddInParameter("AgentList", DbType.String, agentListXml);
                db.AddInParameter("IsIncludeDormant", DbType.Boolean, isIncludeDormant);
                return db.Query<string>();
            }
        }
    }
}