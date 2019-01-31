// -----------------------------------------------------------------------
// <copyright file="ICancelledBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;

    public interface ICancelledBetListDataAccess
    {
        IEnumerable<CustomerCancelBetItem> GetCancelBetCustList(string agentListXml);
    }
}