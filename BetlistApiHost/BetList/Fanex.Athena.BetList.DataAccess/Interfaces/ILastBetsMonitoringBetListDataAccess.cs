// -----------------------------------------------------------------------
// <copyright file="ILastBetsMonitoringBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ILastBetsMonitoringBetListDataAccess
    {
        TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel, IDictionary<int, string> currencyList);

        IList<BetListMatch> GetMatchList(string uspKey);
    }
}