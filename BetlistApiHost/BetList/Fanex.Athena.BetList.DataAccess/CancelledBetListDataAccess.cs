// -----------------------------------------------------------------------
// <copyright file="CancelledBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DataAccess;
    using Fanex.Athena.BetList.Model.DTO;
    using Interfaces;

    public class CancelledBetListDataAccess : BaseDataAccess, ICancelledBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_SelDeleted_UserList";

        public async Task<IEnumerable<CustomerCancelBetItem>> GetCancelBetCustList(string agentListXml)
        {
            using (var db = this.GetObjectDb(UspKey))
            {
                var param = new
                {
                    AgentList = agentListXml
                };

                return (await db.QueryAsync<CustomerCancelBetItem>(param).ConfigureAwait(false)).ToList();
            }
        }
    }
}