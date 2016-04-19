// -----------------------------------------------------------------------
// <copyright file="IMatchDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public interface IMatchDataAccess
    {
        IList<Match> GetForecastMatchListByLeague(int leagueId);

        IList<Match> GetAllRunningMatch(int sportId);

        IList<Match> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate);
    }
}