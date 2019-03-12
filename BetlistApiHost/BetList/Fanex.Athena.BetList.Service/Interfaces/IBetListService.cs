// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Entities;

    [System.CLSCompliant(false)]
    public interface IBetListService
    {
        string GetLeagueName(int leagueId);

        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel);

        TicketResult GetBetListData(BetListMetaModel metaModel);

        List<TicketText> BuildTicketTextList(
            TicketResult ticketResult,
            BetListMetaModel metaModel = null,
            int startIndex = 1,
            bool showOdds = false,
            bool isShowPlayerComm = false);
    }
}