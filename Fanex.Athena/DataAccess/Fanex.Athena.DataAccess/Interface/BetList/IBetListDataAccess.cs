// -----------------------------------------------------------------------
// <copyright file="IBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.BetList
{
    #region Using

    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.DTO.Customer;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.BetList.Core.Entities;

    #endregion Using

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IBetListDataAccess
    {
        TicketResult GetMixParlayDetailTicket(long transId, int liveIndicator);

        SystemParlayData GetSystemParlayDetail(SystemParlayDetailMetaModel metaModel);

        TicketResult GetBetListData(BetListMetaModel metaModel, string uspKey);

        IList<DailyStatement> GetDailyStatementData(BetListMetaModel metaModel, bool allowCustComm);

        IList<CustomerCancelBetItem> GetCancelBetCustList(BetListMetaModel metaModel);

        List<BetListMatch> GetMatchList(string uspKey);

        TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel);
    }
}