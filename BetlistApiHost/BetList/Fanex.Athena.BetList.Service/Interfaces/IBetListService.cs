// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    [CLSCompliant(false)]
    public interface IBetListService
    {
        string GetLeagueName(int leagueId);

        Task<TicketResult> GetMixParlayDetailTicket(MixParlayDetailMetaModel metaModel);

        Task<TicketResult> GetBetListData(BetListMetaModel metaModel);
    }
}