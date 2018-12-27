// -----------------------------------------------------------------------
// <copyright file="CancelledBetListDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Interfaces;

    public class CancelledBetListDataAccess : BaseDataAccess, ICancelledBetListDataAccess
    {
        private const string UspKey = "Lrf_BetList_SelDeleted_UserList";

        public IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel)
        {
            using (var db = this.GetObjectDb(UspKey))
            {
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                return db.Query<CustomerCancelBetItem>().ToList();
            }
        }
    }
}