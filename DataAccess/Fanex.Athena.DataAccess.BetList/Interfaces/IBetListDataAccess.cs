// -----------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;

    [System.CLSCompliant(false)]
    public interface IBetListDataAccess
    {
        TicketResult GetMixParlayDetailTicket(long transId);

        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey);

        IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm);
    }
}