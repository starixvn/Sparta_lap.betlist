// -----------------------------------------------------------------------
// <copyright file="ICancelledBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;

    public interface ICancelledBetListDataAccess
    {
        IEnumerable<CustomerCancelBetItem> GetCancelBetCustList(string agentListXml);
    }
}