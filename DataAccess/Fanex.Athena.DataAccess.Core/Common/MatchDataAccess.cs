// -----------------------------------------------------------------------
// <copyright file="MatchDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using VendorAdapter.Interfaces;

    public class MatchDataAccess : BaseDataAccess, IMatchDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IAppSettings _appSettings;

        public MatchDataAccess(ICacheRefAdapter cacheRefAdapter, IAppSettings appSettings)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _appSettings = appSettings;
        }

        public async Task<IList<Match>> GetForecastMatchListByLeague(int leagueId)
        {
            const string UspName = "Lrf_Forecast_MatchList";
            IList<Match> result = new List<Match>();

            var uspParam = new
            {
                LeagueId = leagueId
            };

            using (var db = GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParam).ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var match = new Match
                        {
                            LeagueId = leagueId,
                            MatchId = reader.ToInt("matchid"),
                            HomeId = reader.ToInt("homeid"),
                            AwayId = reader.ToInt("awayid"),
                            IsNeutral = reader.ToBool("IsNeutral"),
                        };
                        match.MatchName = GetMatchName(match);

                        result.Add(match);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get all running matches.
        /// </summary>
        /// <param name="sportId">Sport ID value.</param>
        /// <returns>The result value.</returns>
        public async Task<IList<Match>> GetAllRunningMatch(int sportId)
        {
            var result = new List<Match>();

            using (var db = GetObjectDb("Lrf_LeagueMatchSelRunning"))
            {
                db.SetParameterValue("sporttype", sportId);
                db.SetParameterValue("IsIncludeTestLeague", !_appSettings.IsProduction);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var awayId = Convert.ToInt32(reader["awayid"]);
                        var match = new Match
                        {
                            MatchId = Convert.ToInt32(reader["matchid"]),
                            LeagueId = Convert.ToInt32(reader["leagueid"]),
                            IsOutRight = awayId == 0,
                            HomeId = Convert.ToInt32(reader["homeid"]),
                            AwayId = awayId
                        };

                        match.MatchName = GetMatchName(match);

                        result.Add(match);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get matches by league id (only for SB)
        /// </summary>
        public async Task<IList<Match>> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate, int sportId = 0)
        {
            var result = new List<Match>();

            using (var db = GetObjectDb("Lrf_SelMatchByLeague"))
            {
                var uspParam = new
                {
                    SportId = sportId,
                    LeagueID = leagueId,
                    FromDate = fromDate,
                    ToDate = toDate
                };
                using (var reader = await db.ExecuteReaderAsync(uspParam).ConfigureAwait(false))
                {
                    while (reader.Read())
                    {
                        var match = new Match
                        {
                            MatchId = Convert.ToInt32(reader["matchid"]),
                            HomeId = reader.ToInt("homeid"),
                            AwayId = reader.ToInt("awayid")
                        };
                        match.MatchName = GetMatchName(match);

                        result.Add(match);
                    }
                }
            }

            return result;
        }

        private string GetMatchName(Match match)
        {
            return _cacheRefAdapter.GetTeamName(match.HomeId) + (match.IsNeutral ? " (N)" : string.Empty)
                                   + " -vs- "
                               + _cacheRefAdapter.GetTeamName(match.AwayId);
        }
    }
}