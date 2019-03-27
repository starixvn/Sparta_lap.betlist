// -----------------------------------------------------------------------
// <copyright file="IMatchDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IMatchDataAccess
    {
        Task<IList<Match>> GetForecastMatchListByLeague(int leagueId);

        Task<IList<Match>> GetAllRunningMatch(int sportId);

        Task<IList<Match>> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate, int sportId = 0);
    }
}