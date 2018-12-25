// -----------------------------------------------------------------------
// <copyright file="ILastBetsMonitoringBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList.Interfaces
{
    using Fanex.Athena.Model.BetList.DTO;
    using Fanex.Athena.Model.BetList.MetaModel;
    using System.Collections.Generic;

    public interface ILastBetsMonitoringBetListService
    {
        TicketResult GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel, IDictionary<int, string> currencyList);
    }
}