// -----------------------------------------------------------------------
// <copyright file="IBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.BetList
{
    #region

    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.BetList.Core.Entities;

    #endregion

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    [System.CLSCompliant(false)]
    public interface IBetListService
    {
        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        SystemParlayData GetMicroGamingRngDetail(MicroGamingRngDetailMetaModel metaModel);

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