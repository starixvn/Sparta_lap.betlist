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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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

                        if (type == typeof(Sportbook))
                        {
                            return (IList<T>)ReadSportbookMatches(reader);
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
            catch (SqlException sqlException)
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
                        var dataParlay = new List<Parlay>();

                        while (reader.Read())
                        {
                            var leagueId = reader.ToInt("LeagueID");
                            var parlay = dataParlay.SingleOrDefault(x => x.LeagueId == leagueId);
                            if (parlay == null)
                            {
                                parlay = new Parlay()
                                   {
                                       LeagueId = leagueId,
                                       SportId = reader.ToInt("sporttype")
                                   };
                            }

                            parlay.Sportbook.Add(new Sportbook
                                                      {
                                                          AwayId = reader.ToInt("awayID"),
                                                          HomeId = reader.ToInt("homeID"),
                                                          BetStatus =
                                                              reader["betstatus"] != DBNull.Value
                                                                  ? reader.ToString("betstatus") : string.Empty,
                                                          LeagueId = leagueId,
                                                          MatchId = reader.ToInt("matchid"),
                                                          EventStatus = reader.ToString("eventstatus"),
                                                          FinalAwayScore =
                                                              reader["finalawayscore"] != DBNull.Value
                                                                  ? reader.ToInt("finalawayscore") : (int?)null,
                                                          HtAwayScore =
                                                              reader["htawayscore"] != DBNull.Value
                                                                  ? reader.ToInt("htawayscore") : (int?)null,
                                                          Color = reader.TryGetOrdinal("lcolor") >= 0 &&
                                                              reader["lcolor"] != DBNull.Value
                                                                  ? reader.ToString("lcolor") : string.Empty,
                                                          FinalHomeScore =
                                                              reader["finalhomescore"] != DBNull.Value
                                                                  ? reader.ToInt("finalhomescore") : (int?)null,
                                                          HtHomeScore =
                                                              reader["finalhomescore"] != DBNull.Value
                                                                  ? reader.ToInt("hthomescore") : (int?)null,
                                                          EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                                          IsNeutral = reader.ToBool("IsNeutral"),
                                                          IsPending = reader.ToBool("IsPending"),
                                                          KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                                                          MatchCode = reader.ToString("matchcode"),
                                                          GoalRecord = reader.ToString("goalrecord"),
                                                          ResultValue = string.Empty,
                                                          IsHTCal = reader.ToBool("IsHTCal"),
                                                          Calculate = reader.ToBool("Calculate"),
                                                          RefNo = refNo.ToString(CultureInfo.InvariantCulture),
                                                      });
                        }

                        return dataParlay;
                    }
                }
            }
            catch (SqlException sqlException)
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
            catch (SqlException sqlException)
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

                        if (type == typeof(Sportbook))
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
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #region Binding

        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <returns>The sport-book data list.</returns>        
        private static IList<Sportbook> ReadSportbookMatches(IDataReader reader)
        {
            var dataList = new List<Sportbook>();
            while (reader.Read())
            {
                dataList.Add(new Sportbook
                                {
                                    AwayId = reader.ToInt("awayID"),
                                    HomeId = reader.ToInt("homeID"),
                                    BetStatus =
                                        reader["betstatus"] != DBNull.Value ? reader.ToString("betstatus") : string.Empty,
                                    LeagueId = reader.ToInt("leagueid"),
                                    MatchId = reader.ToInt("matchid"),
                                    EventStatus = reader.ToString("eventstatus"),
                                    FinalAwayScore =
                                        reader["finalawayscore"] != DBNull.Value
                                            ? reader.ToInt("finalawayscore") : (int?)null,
                                    HtAwayScore =
                                        reader["htawayscore"] != DBNull.Value ? reader.ToInt("htawayscore") : (int?)null,
                                    Color = reader.TryGetOrdinal("lcolor") >= 0 && reader["lcolor"] != DBNull.Value ? reader.ToString("lcolor") : string.Empty,
                                    FinalHomeScore =
                                        reader["finalhomescore"] != DBNull.Value
                                            ? reader.ToInt("finalhomescore") : (int?)null,
                                    HtHomeScore =
                                        reader["hthomescore"] != DBNull.Value
                                            ? reader.ToInt("hthomescore") : (int?)null,
                                    EventDate = DateTime.Parse(reader.ToString("eventdate")),
                                    IsNeutral = reader.ToBool("IsNeutral"),
                                    IsPending = reader.ToBool("IsPending"),
                                    KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                                    MatchCode = reader.ToString("matchcode"),
                                    GoalRecord = reader.ToString("goalrecord"),
                                    ResultValue = string.Empty,
                                    IsHTCal = reader.ToBool("IsHTCal"),
                                    Calculate = reader.ToBool("Calculate"),
                                    RefNo = string.Empty
                                });
            }

            return dataList;
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
                            IsPending = reader.TryGetOrdinal("isrefund") >= 0 && reader.ToString("isrefund") != "0",
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
                                    FinalAwayScore =
                                        reader["finalawayscore"] != DBNull.Value ? reader.ToInt("finalawayscore") : (int?)null,
                                    FinalHomeScore =
                                        reader["finalhomescore"] != DBNull.Value ? reader.ToInt("finalhomescore") : (int?)null,
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
                                    IsFavor = reader["IsFavor"] != DBNull.Value && reader.ToString("IsFavor").Equals("1") ? true : false,
                                    IsSCR = reader["IsSCR"] != DBNull.Value && reader.ToString("IsSCR").Equals("1") ? true : false,
                                    Place = reader.ToInt("place"),
                                    PlaceOdds = reader.ToDecimal("placeodds"),
                                    WinOdds = reader.ToDecimal("odds"),
                                    RaceName = reader["racename"] != DBNull.Value ? reader.ToInt("racename") : 0,
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