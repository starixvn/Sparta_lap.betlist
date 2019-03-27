// -----------------------------------------------------------------------
// <copyright file="IMatchService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IMatchService
    {
        Task<IList<Match>> GetAllRunningMatch(int sportId, string languageCode);

        Task<IList<Match>> GetForecastMatchListByLeague(int leagueId);

        Task<IList<Match>> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate);
    }
}