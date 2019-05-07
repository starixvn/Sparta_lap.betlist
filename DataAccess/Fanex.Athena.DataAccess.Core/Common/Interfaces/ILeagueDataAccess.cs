// -----------------------------------------------------------------------
// <copyright file="ILeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.Core.MetaModel.Common;

    public interface ILeagueDataAccess
    {
        Task<IEnumerable<League>> GetLeaguesBySport(LeagueMetaModel metaModel);

        Task<IEnumerable<League>> GetRunningLeagues(string agentList, string betTypes);
    }
}