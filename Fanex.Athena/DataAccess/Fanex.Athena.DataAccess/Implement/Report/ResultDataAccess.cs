// -----------------------------------------------------------------------
// <copyright file="ResultDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// Result data access.
    /// </summary>
    public class ResultDataAccess : IResultDataAccess
    {
        /// <summary>
        /// Get out right.
        /// </summary>
        /// <param name="metal">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<OutRight> GetOutRight(ResultMetalModel metal)
        {
            const string UspName = "Lrf_Result_Outright";
            var uspParams = new
            {
                fromdate = metal.FromDate,
                todate = metal.ToDate,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var data = new List<OutRight>();

                        while (reader.Read())
                        {
                            data.Add(new OutRight
                                            {
                                                EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                                SportType = reader.ToInt("sporttype"),
                                                LeagueId = reader.ToInt("leagueid"),
                                                TeamId = reader.ToInt("teamid"),
                                                Status = reader.ToString("status")
                                            });
                        }

                        return data;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get sports.
        /// </summary>
        /// <param name="dateValue">Date value.</param>
        /// <param name="licenseeId">The licensee Id.</param>
        /// <returns>The result value.</returns>
        public IList<Sport> GetSports(DateTime dateValue, int licenseeId)
        {
            const string UspName = "Lrf_Result_SportSelByDate";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, dateValue);
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                    using (var reader = db.ExecuteReader())
                    {
                        var data = new List<Sport>();
                        while (reader.Read())
                        {
                            data.Add(new Sport(reader["sporttype"].ToString()));
                        }

                        return data;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get leagues.
        /// </summary>
        /// <param name="dateValue">Date value.</param>
        /// <param name="sportId">Sport ID value.</param>
        /// <returns>The result value.</returns>
        public IList<League> GetLeagues(DateTime dateValue, int sportId)
        {
            const string UspName = "Lrf_Result_LeagueSelByDate";
            var uspParams = new
            {
                eventdate = dateValue,
                sporttype = sportId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var data = new List<League>();
                        while (reader.Read())
                        {
                            data.Add(new League { LeagueId = Convert.ToInt32(reader["leagueid"]) });
                        }

                        return data;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Number Game match.
        /// </summary>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadNumberGameMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get racing match.
        /// </summary>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<Racing> GetRacingMatches(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_RaceSelByMeeting";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadRacingMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Match.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="matchId">Match ID value.</param>
        /// <param name="raceNo">Race Number value.</param>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="bettypeId">Bet type id.</param>
        /// <param name="betId">The bet id.</param>
        /// <returns>The result value.</returns>
        public IList<T> GetPopUpMatches<T>(int matchId, int raceNo, int licenseeId, int userId, int bettypeId, long? betId)
        {
            const string UspName = "Lrf_BetList_SelMatchResult";
            var uspParams = new
            {
                matchid = matchId,
                raceno = raceNo > -1 ? raceNo.ToString(CultureInfo.InvariantCulture) : null,
                licenseeid = licenseeId,
                userid = userId,
                bettype = bettypeId,
                betid = betId
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var type = typeof(T);

                        if (type == typeof(Sportsbook))
                        {
                            return (IList<T>)ReadSportbookMatches(reader, bettypeId, betId.HasValue ? betId.Value : 0);
                        }
                        else if (type == typeof(LiveCasino))
                        {
                            return (IList<T>)ReadLiveCasinoMatches(reader);
                        }
                        else if (type == typeof(Racing))
                        {
                            return (IList<T>)ReadRacingMatches(reader);
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("The type '{0}' is not supported.", type.FullName));
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Parlay Match.
        /// </summary>
        /// <param name="refNo">Reference Number value.</param>
        /// <returns>The result value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public IList<Parlay> GetPopUpParalayMatch(long refNo)
        {
            const string UspName = "Lrf_BetList_SelMatchResultParlay";
            var uspParams = new
            {
                refno = refNo
            };
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var results = new List<Parlay>();

                        while (reader.Read())
                        {
                            var leagueId = reader.ToInt("LeagueID");
                            var parlay = results.SingleOrDefault(x => x.LeagueId == leagueId);
                            if (parlay == null)
                            {
                                parlay = new Parlay()
                                   {
                                       LeagueId = leagueId,
                                       SportId = reader.ToInt("sporttype")
                                   };

                                results.Add(parlay);
                            }

                            parlay.Sportbook.Add(new Sportsbook
                                                      {
                                                          Bettype = reader.FieldExists("bettype") ? reader.ToInt("bettype") : 0,
                                                          BetID = reader.FieldExists("betcheck") ? reader.ToString("betcheck") : string.Empty,
                                                          AwayId = reader.ToInt("awayID"),
                                                          HomeId = reader.ToInt("homeID"),
                                                          BetStatus = reader["betstatus"] != DBNull.Value ? reader.ToString("betstatus") : string.Empty,
                                                          LeagueId = leagueId,
                                                          MatchId = reader.ToInt("matchid"),
                                                          EventStatus = reader.ToString("eventstatus"),
                                                          FinalAwayScore = reader.ToNullableInt("finalawayscore"),
                                                          HtAwayScore = reader.ToNullableInt("htawayscore"),
                                                          Color = reader.FieldExists("lcolor") &&
                                                              reader["lcolor"] != DBNull.Value ? reader.ToString("lcolor") : string.Empty,
                                                          FinalHomeScore = reader.ToNullableInt("finalhomescore"),
                                                          HtHomeScore = reader.ToNullableInt("hthomescore"),
                                                          EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                                          IsNeutral = reader.ToBool("IsNeutral"),
                                                          IsPending = reader.ToBool("IsPending"),
                                                          KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                                                          MatchCode = reader.ToString("matchcode"),
                                                          GoalRecord = reader.ToString("goalrecord"),
                                                          IsHTCal = reader.ToBool("IsHTCal"),
                                                          Calculate = reader.ToBool("Calculate"),
                                                          RefNo = refNo.ToString(CultureInfo.InvariantCulture),
                                                      });
                        }

                        var gameDetails = GetGameDetails(reader);

                        if (gameDetails.Count > 0)
                        {
                            foreach (var result in results)
                            {
                                foreach (var item in result.Sportbook)
                                {
                                    item.GameDetails.AddRange(gameDetails.Where(i => i.MatchId == item.MatchId));
                                }
                            }
                        }

                        return results;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Number game match.
        /// </summary>
        /// <param name="matchId">Match ID value.</param>
        /// <returns>The result value.</returns>
        public IList<NumberGame> GetPopUpNumberGameMatch(int matchId)
        {
            const string UspName = "Lrf_BetList_SelMatchResultNumberGame";
            var uspParams = new
            {
                matchid = matchId
            };
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadNumberGameMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get match.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public IList<T> GetMatches<T>(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                sporttype = metalModel.SportId,
                leagueid = metalModel.LeagueId,
                isshowall = true
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var type = typeof(T);

                        if (type == typeof(Sportsbook))
                        {
                            return (IList<T>)ReadSportbookMatches(reader);
                        }
                        else if (type == typeof(LiveCasino))
                        {
                            return (IList<T>)ReadLiveCasinoMatches(reader);
                        }
                        else
                        {
                            throw new NotSupportedException(string.Format("The type '{0}' is not supported.", type.FullName));
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get game detail.
        /// </summary>
        /// <param name="matchId">The match ID.</param>
        /// <returns>The SPORTSBOOK data.</returns>
        [System.CLSCompliant(false)]
        public Sportsbook GetSportsbookGameDetail(int matchId)
        {
            using (var db = new ObjectDb("Lrf_Result_MatchSelByLeague_Detail"))
            {
                db.AddInParameter("matchid", DbType.Int32, matchId);
                using (var reader = db.ExecuteReader())
                {
                    var result = new Sportsbook();
                    while (reader.Read())
                    {
                        result = BindSportsbook(reader);
                    }

                    result.GameDetails = GetGameDetails(reader).ToList();

                    return result;
                }
            }
        }

        #region Binding

        /// <summary>
        /// Bind game detail value.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>The game detail value.</returns>
        private static IList<GameDetail> GetGameDetails(IDataReader reader)
        {
            var gameDetails = new List<GameDetail>();

            if (reader.NextResult())
            {
                while (reader.Read())
                {
                    gameDetails.Add(new GameDetail
                    {
                        MatchId = int.Parse(reader["matchid"].ToString()),
                        ResourceId = reader["ResourceId"].ToString(),
                        HomeValue = reader["HomeValue"].ToString(),
                        AwayValue = reader["AwayValue"].ToString(),
                        ResultStatusID = int.Parse(reader["ResultStatusID"].ToString()),
                        SettlementStatusID = int.Parse(reader["SettlementStatusID"].ToString()),
                        OtherInfo = reader["OtherInfo"].ToString()
                    });
                }
            }

            return gameDetails;
        }

        /// <summary>
        /// Bind sports book.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>A sports book DTO.</returns>
        private static Sportsbook BindSportsbook(IDataReader reader)
        {
            return new Sportsbook
                                {                                    
                                    AwayId = reader.ToInt("awayID"),
                                    HomeId = reader.ToInt("homeID"),
                                    BetStatus = reader["betstatus"] != DBNull.Value ? reader.ToString("betstatus") : string.Empty,
                                    LeagueId = reader.ToInt("leagueid"),
                                    MatchId = reader.ToInt("matchid"),
                                    EventStatus = reader.ToString("eventstatus"),
                                    FinalAwayScore = reader.ToNullableInt("finalawayscore"),
                                    HtAwayScore = reader.ToNullableInt("htawayscore"),
                                    Color = reader.FieldExists("lcolor") && reader["lcolor"] != DBNull.Value ? reader.ToString("lcolor") : string.Empty,
                                    FinalHomeScore = reader.ToNullableInt("finalhomescore"),
                                    HtHomeScore = reader.ToNullableInt("hthomescore"),
                                    EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                    IsNeutral = reader.ToBool("IsNeutral"),
                                    IsPending = reader.ToBool("IsPending"),
                                    KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                                    MatchCode = reader.ToString("matchcode"),
                                    GoalRecord = string.Empty,
                                    IsHTCal = reader.ToBool("IsHTCal"),
                                    Calculate = reader.ToBool("Calculate"),
                                    RefNo = string.Empty
                                };
        }

        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="bettype">The Bet type ID.</param>
        /// <param name="betid">The Bet ID.</param>
        /// <returns>The sport-book data list.</returns>        
        private static IList<Sportsbook> ReadSportbookMatches(IDataReader reader, int bettype = 0, long betid = 0)
        {
            var results = new List<Sportsbook>();

            while (reader.Read())
            {
                var result = BindSportsbook(reader);
                result.BetID = betid.ToString();
                result.Bettype = bettype;

                results.Add(result);
            }

            var gameDetails = GetGameDetails(reader);
            if (gameDetails.Count > 0)
            {
                foreach (var result in results)
                {
                    result.GameDetails = gameDetails.Where(i => i.MatchId == result.MatchId).ToList();
                }
            }

            return results;
        }

        /// <summary>
        /// Mapping data to NG DTO.
        /// </summary>
        /// <param name="reader">The data table.</param>
        /// <returns>The NG data list.</returns>
        private static IList<NumberGame> ReadNumberGameMatches(IDataReader reader)
        {
            var dataList = new List<NumberGame>();

            while (reader.Read())
            {
                var matchId = reader.ToInt("matchid");
                var existMatch = dataList.SingleOrDefault(i => i.MatchId == matchId);
                if (existMatch != null)
                {
                    existMatch.Seg.Add(reader.ToInt("seq"));
                    existMatch.Ball.Add(reader.ToInt("number"));
                }
                else
                {
                    dataList.Add(
                        new NumberGame
                        {
                            LeagueId = reader.ToInt("leagueid"),
                            MatchId = reader.ToInt("matchid"),
                            EventStatus = reader.ToString("eventstatus"),
                            EventDate = DateTime.Parse(reader.ToString("eventdate")),
                            IsPending = reader.FieldExists("isrefund") && reader.ToString("isrefund") != "0",
                            KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                            MatchCode = reader.ToString("matchcode"),
                            Seg = new List<int> { reader.ToInt("seq") },
                            Ball = new List<int> { reader.ToInt("number") }
                        });
                }
            }

            return dataList.OrderBy(x => x.LeagueId).OrderBy(x => x.MatchId).ToList();
        }

        /// <summary>
        /// Mapping data to Live casino DTO.
        /// </summary>
        /// <param name="reader">The data table.</param>
        /// <returns>The live casino data list.</returns>        
        private static IList<LiveCasino> ReadLiveCasinoMatches(IDataReader reader)
        {
            var dataList = new List<LiveCasino>();

            while (reader.Read())
            {
                dataList.Add(new LiveCasino
                                {
                                    ShowTime = reader.ToString("showtime"),
                                    LeagueId = reader.ToInt("leagueid"),
                                    EventStatus = reader.ToString("eventstatus"),
                                    FinalAwayScore = reader.ToNullableInt("finalawayscore"),
                                    FinalHomeScore = reader.ToNullableInt("finalhomescore"),
                                    EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                    IsPending = reader.ToBool("IsPending"),
                                    KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                                    IsHTCal = reader.ToBool("IsHTCal"),
                                    Calculate = reader.ToBool("Calculate"),
                                    BetStatus = reader["betstatus"] != DBNull.Value ? reader.ToString("betstatus") : string.Empty,
                                    Bettype = reader.ToInt("homeid2")
                                });
            }

            return dataList;
        }

        /// <summary>
        /// Mapping racing data from data table.
        /// </summary>
        /// <param name="reader">The data table.</param>
        /// <returns>The racing data list.</returns>
        private static IList<Racing> ReadRacingMatches(IDataReader reader)
        {
            var dataList = new List<Racing>();

            while (reader.Read())
            {
                dataList.Add(
                                new Racing
                                {
                                    LeagueId = reader.ToInt("meetingid"),
                                    EngineId = reader.ToString("HorseId"),
                                    EngineNo = reader.ToString("HorseNo"),
                                    IsFavor = reader["IsFavor"] != DBNull.Value && reader.ToString("IsFavor").Equals("1"),
                                    IsSCR = reader["IsSCR"] != DBNull.Value && reader.ToString("IsSCR").Equals("1"),
                                    Place = reader.ToInt("place"),
                                    PlaceOdds = reader.ToDecimal("placeodds"),
                                    WinOdds = reader.ToDecimal("odds"),
                                    RaceName = (int)reader.ToNullableInt("racename", 0),
                                    KickOffTime = reader["kickofftime"] != DBNull.Value ? DateTime.Parse(reader.ToString("kickofftime")) : DateTime.MinValue,
                                    Favourite = reader["IsFavor"] != DBNull.Value ? reader.ToString("IsFavor") : string.Empty,
                                    RowCount = reader.ToInt("rowcount"),
                                    SCROfMatch = reader.ToString("SCR"),
                                    TFOfMatch = reader.ToString("TF")
                                });
            }

            return dataList;
        }

        #endregion Biding
    }
}