// -----------------------------------------------------------------------
// <copyright file="ILastBetsMonitoringBetListDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;

    public interface ILastBetsMonitoringBetListDataAccess
    {
        Task<TicketResult> GetLastBetsMonitoringData(LastBetsMonitoringMetaModel metaModel, IDictionary<int, string> currencyList);

        Task<IList<BetListMatch>> GetMatchList(string uspKey);
    }
}