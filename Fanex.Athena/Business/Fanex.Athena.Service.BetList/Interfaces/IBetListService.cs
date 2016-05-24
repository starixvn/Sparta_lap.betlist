// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList.Interfaces
{
    #region

    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Fanex.BetList.Core.Entities;

    #endregion

    [System.CLSCompliant(false)]
    public interface IBetListService
    {
        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetMixParlayDetailTicket(long transId);

        TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey);

        IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool isAllowCustComm);

        IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel);

        /// <summary>
        /// Get last bets monitoring data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The ticket result.</returns>
        TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel);
    }
}