// -----------------------------------------------------------------------
// <copyright file="MatchDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------
namespace Fanex.Athena.DataAccess.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class MatchDataAccess : BaseDataAccess, IMatchDataAccess
    {
        /// <summary>
        /// Get match list by league.
        /// </summary>
        /// <param name="leagueId">League ID value.</param>
        /// <returns>The result value.</returns>
        public IList<Match> GetForecastMatchListByLeague(int leagueId)
        {
            var uspName = "Lrf_Forecast_MatchList";
            IList<Match> result = new List<Match>();

            var uspParam = new
            {
                LeagueId = leagueId
            };

            using (var db = new ObjectDb(uspName))
            {
                using (var reader = db.ExecuteReader(uspParam))
                {
                    if (reader != null)
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

                            result.Add(match);
                        }
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
        public IList<Match> GetAllRunningMatch(int sportId)
        {
            var uspName = "Lrf_LeagueMatchSelRunning";
            var result = new List<Match>();

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("sporttype", System.Data.DbType.Int32, sportId);
                using (var reader = db.ExecuteReader())
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

                        result.Add(match);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get matches by league id.
        /// </summary>
        /// <param name="leagueId">League id.</param>
        /// <param name="fromDate">From date value.</param>
        /// <param name="toDate">To date value.</param>
        /// <returns>List of match.</returns>
        public IList<Match> GetMatchesByLeague(int leagueId, DateTime fromDate, DateTime toDate)
        {
            var uspName = "Lrf_SelMatchByLeague";
            var result = new List<Match>();

            using (var db = new ObjectDb(uspName))
            {
                var uspParam = new
                {                    
                    LeagueID = leagueId,
                    FromDate = fromDate,
                    ToDate = toDate
                };
                using (var reader = db.ExecuteReader(uspParam))
                {
                    while (reader.Read())
                    {                        
                        var match = new Match
                        {
                            MatchId = Convert.ToInt32(reader["matchid"]),                            
                            HomeId = reader.ToInt("homeid"),
                            AwayId = reader.ToInt("awayid")
                        };

                        result.Add(match);
                    }
                }
            }

            return result;
        }
    }
}