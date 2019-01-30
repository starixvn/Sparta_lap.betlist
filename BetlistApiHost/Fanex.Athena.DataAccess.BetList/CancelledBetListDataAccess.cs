// -----------------------------------------------------------------------
// <copyright file="CancelledBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.Model.BetList.DTO;
    using Interfaces;

    public class CancelledBetListDataAccess : BaseDataAccess, ICancelledBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_SelDeleted_UserList";

        public IEnumerable<CustomerCancelBetItem> GetCancelBetCustList(string agentListXml)
        {
            using (var db = this.GetObjectDb(UspKey))
            {
                var param = new
                {
                    AgentList = agentListXml
                };

                return db.Query<CustomerCancelBetItem>(param).ToList();
            }
        }
    }
}