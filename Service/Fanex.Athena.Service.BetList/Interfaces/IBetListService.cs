// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;

    [System.CLSCompliant(false)]
    public interface IBetListService
    {
        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetMixParlayDetailTicket(IDictionary<int, string> currencyList, int licenseeId, long transId, int custId);

        TicketResult GetBetListData(BetListMetaModel metaModel, IDictionary<int, string> currencyList, string uspKey);
    }
}