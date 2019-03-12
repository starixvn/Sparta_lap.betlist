// -----------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Entities;

    [System.CLSCompliant(false)]
    public interface IBetListDataAccess
    {
        TicketResult GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel);

        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetBetListData(BetListMetaModel metaModel);
    }
}