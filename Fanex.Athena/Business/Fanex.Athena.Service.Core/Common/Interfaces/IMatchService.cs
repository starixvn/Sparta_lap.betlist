// -----------------------------------------------------------------------
// <copyright file="IMatchService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMatchService
    {
        /// <summary>
        /// Get all running matches.
        /// </summary>
        /// <param name="sportId">The sport id.</param>
        /// <returns>The running match list.</returns>
        IList<Match> GetAllRunningMatch(int sportId);

        /// <summary>
        /// Get match by league id.
        /// </summary>
        /// <param name="leagueId">The league id.</param>
        /// <returns>The match list.</returns>
        IList<Match> GetForecastMatchListByLeague(int leagueId);

        IList<Match> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate);
    }
}