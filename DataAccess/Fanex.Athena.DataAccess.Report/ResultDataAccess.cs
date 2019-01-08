// -----------------------------------------------------------------------
// <copyright file="ResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using Constant.Helper;
    using Core;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Result.Core.Happy5.DTO;
    using VendorAdapter.Interfaces;

    [CLSCompliant(false)]
    public class ResultDataAccess : BaseDataAccess, IResultDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IAppSettings _appSettings;

        public ResultDataAccess(
            IBalanceUpDataAccess balanceUpDataAccess,
            ICacheRefAdapter cacheRefAdapter,
            IAppSettings appSettings)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _cacheRefAdapter = cacheRefAdapter;
            _appSettings = appSettings;
        }

        public IList<Outright> GetOutRight(ResultMetalModel metal)
        {
            const string UspName = "Lrf_Result_Outright";
            var uspParams = new
            {
                fromdate = metal.FromDate,
                todate = metal.ToDate,
                isshowall = !_appSettings.IsProduction
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var data = new List<Outright>();

                        while (reader.Read())
                        {
                            data.Add(new Outright
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
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        public IList<Sport> GetSports(DateTime dateValue, int licenseeId)
        {
            const string UspName = "Lrf_Result_SportSelByDate";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("eventdate", DbType.DateTime, dateValue);
                db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                db.AddInParameter("movebetsday", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
                using (var reader = db.ExecuteReader())
                {
                    var data = new List<Sport>();
                    while (reader.Read())
                    {
                        var sportId = reader.ToInt("sporttype");
                        data.Add(new Sport
                        {
                            SportId = sportId,
                            SportName = _cacheRefAdapter.GetSportName(sportId)
                        });
                    }

                    return data;
                }
            }
        }

        public IList<League> GetLeagues(DateTime dateValue, int sportId)
        {
            const string UspName = "Lrf_Result_LeagueSelByDate";
            var uspParams = new
            {
                eventdate = dateValue,
                sporttype = sportId,
                isshowall = !_appSettings.IsProduction,
                movebetsday = _balanceUpDataAccess.GetBalanceUp().MoveBetsDay
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var data = new List<League>();
                        while (reader.Read())
                        {
                            var leagueId = Convert.ToInt32(reader["leagueid"]);
                            data.Add(new League
                            {
                                LeagueId = leagueId,
                                LeagueName = _cacheRefAdapter.GetLeagueName(leagueId)
                            });
                        }

                        return data;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        public IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadNumberGameMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        public IList<Happy5> GetHappy5Matches(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadHappy5Matches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get data for VS
        /// </summary>
        public IList<Racing> GetVirtualSportMatches(ResultMetalModel metalModel)
        {
            const string UspName = "Lrf_Result_RaceSelByMeeting";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadVirtualSportMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        public IList<T> GetPopUpMatches<T>(ResultPopupMetalModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelMatchResult";
            var uspParams = new
            {
                matchid = metaModel.MatchId,
                raceno = metaModel.RaceNo.HasValue ? metaModel.RaceNo.Value.ToString(CultureInfo.InvariantCulture) : null,
                bettype = metaModel.Bettype,
                betid = metaModel.BetId
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var type = typeof(T);

                        if (type == typeof(Sportsbook))
                        {
                            return (IList<T>)ReadSportbookMatches(reader, metaModel);
                        }
                        else if (type == typeof(Racing))
                        {
                            return (IList<T>)ReadVirtualSportMatches(reader);
                        }
                        else
                        {
                            throw new NotSupportedException($"The type '{type.FullName}' is not supported.");
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        /// <summary>
        /// Get Popup Parlay Match.
        /// </summary>
        /// <param name="refNo">Reference Number value.</param>
        /// <returns>The result value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        public IList<Parlay> GetPopUpParalayMatch(long refNo)
        {
            const string UspName = "Lrf_BetList_SelMatchResultParlay";
            var uspParams = new { refno = refNo };
            try
            {
                using (var db = this.GetObjectDb(UspName))
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
                                parlay = new Parlay
                                {
                                    LeagueId = leagueId,
                                    SportId = reader.ToInt("sporttype")
                                };

                                results.Add(parlay);
                            }

                            parlay.Sportbook.Add(ReadSportsbook(reader, refNo, leagueId));
                        }

                        AddGameDetails(reader, results);

                        return results;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        private static void AddGameDetails(IDataReader reader, IEnumerable<Parlay> results)
        {
            var gameDetails = GetGameDetails(reader);

            if (!gameDetails.Any())
            {
                return;
            }

            foreach (var result in results)
            {
                foreach (var item in result.Sportbook)
                {
                    item.GameDetails.AddRange(gameDetails.Where(i => i.MatchId == item.MatchId));
                }
            }
        }

        private static Sportsbook ReadSportsbook(IDataReader reader, long refNo, int leagueId)
        {
            return new Sportsbook
            {
                Bettype = reader.HasColumn("bettype") ? reader.ToInt("bettype") : 0,
                BetID = reader.HasColumn("betcheck") ? reader.ToString("betcheck") : string.Empty,
                AwayId = reader.ToInt("awayID"),
                HomeId = reader.ToInt("homeID"),
                BetStatus = reader["betstatus"] != DBNull.Value ? reader.ToString("betstatus") : string.Empty,
                LeagueId = leagueId,
                MatchId = reader.ToInt("matchid"),
                EventStatus = reader.ToString("eventstatus"),
                FinalAwayScore = reader.ToNullableInt("finalawayscore"),
                HtAwayScore = reader.ToNullableInt("htawayscore"),
                Color = reader.HasColumn("lcolor") && reader["lcolor"] != DBNull.Value ? reader.ToString("lcolor") : string.Empty,
                FinalHomeScore = reader.ToNullableInt("finalhomescore"),
                HtHomeScore = reader.ToNullableInt("hthomescore"),
                EventDate = DateTime.Parse(reader.ToString("eventdate")),
                IsNeutral = reader.ToBool("IsNeutral"),
                KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                MatchCode = reader.ToString("matchcode"),
                GoalRecord = reader.ToString("goalrecord"),
                IsHTCal = reader.ToBool("IsHTCal"),
                Calculate = reader.ToBool("Calculate"),
                RefNo = refNo.ToString(CultureInfo.InvariantCulture),
                SettlementStatus = reader.ToByte("SettlementStatus")
            };
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
                matchId
            };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadNumberGameMatches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
            }
        }

        public IList<Happy5> GetPopUpHappy5Match(int matchId)
        {
            const string UspName = "Lrf_BetList_SelMatchResultNumberGame";
            var uspParams = new
            {
                matchId
            };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return ReadHappy5Matches(reader);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
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
                movebetsday = _balanceUpDataAccess.GetBalanceUp().MoveBetsDay
            };

            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var type = typeof(T);

                        if (type == typeof(Sportsbook))
                        {
                            return (IList<T>)ReadSportbookMatches(reader);
                        }
                        else
                        {
                            throw new NotSupportedException($"The type '{type.FullName}' is not supported.");
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, uspParams);
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
            using (var db = this.GetObjectDb("Lrf_Result_MatchSelByLeague_Detail"))
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
                    var resultStatusId = reader["ResultStatusID"];
                    var settlementStatusId = reader["SettlementStatusID"];

                    gameDetails.Add(new GameDetail
                    {
                        MatchId = int.Parse(reader["matchid"].ToString()),
                        ResourceId = reader["ResourceId"].ToString(),
                        HomeValue = reader["HomeValue"].ToString(),
                        AwayValue = reader["AwayValue"].ToString(),
                        ResultStatusID = resultStatusId == DBNull.Value ? 0 : int.Parse(resultStatusId.ToString()),
                        SettlementStatusID = settlementStatusId == DBNull.Value ? 0 : int.Parse(settlementStatusId.ToString()),
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
                Color = reader.HasColumn("lcolor") && reader["lcolor"] != DBNull.Value ? reader.ToString("lcolor") : string.Empty,
                FinalHomeScore = reader.ToNullableInt("finalhomescore"),
                HtHomeScore = reader.ToNullableInt("hthomescore"),
                EventDate = DateTime.Parse(reader.ToString("eventdate")),
                IsNeutral = reader.ToBool("IsNeutral"),
                KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                MatchCode = reader.ToString("matchcode"),
                GoalRecord = reader.ToString("goalrecord"),
                IsHTCal = reader.ToBool("IsHTCal"),
                Calculate = reader.ToBool("Calculate"),
                RefNo = string.Empty,
                LiveAwayScore = reader.HasColumn("LiveAwayScore") ? reader.ToNullableInt("LiveAwayScore") : null,
                LiveHomeScore = reader.HasColumn("LiveHomeScore") ? reader.ToNullableInt("LiveHomeScore") : null,
                SettlementStatus = reader.ToByte("SettlementStatus")
            };
        }

        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="reader">The data reader.</param>
        /// <param name="bettype">The Bet type ID.</param>
        /// <param name="betid">The Bet ID.</param>
        /// <returns>The sport-book data list.</returns>
        private static IList<Sportsbook> ReadSportbookMatches(IDataReader reader, ResultPopupMetalModel metaModel = null)
        {
            var results = new List<Sportsbook>();

            while (reader.Read())
            {
                var result = BindSportsbook(reader);

                if (metaModel != null)
                {
                    result.BetID = BetTypeHelper.IsFastMarket(metaModel.Bettype) ? metaModel.Handicap1.ToString() : (metaModel.BetId ?? 0).ToString();
                    result.Bettype = metaModel.Bettype;
                    result.Handicap2 = metaModel.Handicap2;
                }
                else
                {
                    result.BetID = "0";
                }

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
                            IsPending = reader.HasColumn("isrefund") && reader.ToString("isrefund") != "0",
                            KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                            MatchCode = reader.ToString("matchcode"),
                            Seg = new List<int> { reader.ToInt("seq") },
                            Ball = new List<int> { reader.ToInt("number") }
                        });
                }
            }

            return dataList.OrderBy(x => x.LeagueId).ThenBy(x => x.MatchId).ToList();
        }

        private static IList<Happy5> ReadHappy5Matches(IDataReader reader)
        {
            var dataList = new List<Happy5>();

            while (reader.Read())
            {
                var matchId = reader.ToInt("matchid");
                var existMatch = dataList.SingleOrDefault(i => i.MatchId == matchId);
                if (existMatch != null)
                {
                    existMatch.Seq.Add(reader.ToInt("seq"));
                    existMatch.Ball.Add(reader.ToInt("number"));
                }
                else
                {
                    dataList.Add(
                        new Happy5
                        {
                            LeagueId = reader.ToInt("leagueid"),
                            MatchId = reader.ToInt("matchid"),
                            EventStatus = reader.ToString("eventstatus"),
                            EventDate = DateTime.Parse(reader.ToString("eventdate")),
                            IsPending = reader.HasColumn("isrefund") && reader.ToString("isrefund") != "0",
                            KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                            MatchCode = reader.ToString("matchcode"),
                            Seq = new List<int> { reader.ToInt("seq") },
                            Ball = new List<int> { reader.ToInt("number") }
                        });
                }
            }

            return dataList.OrderBy(x => x.LeagueId).ThenBy(x => x.MatchId).ToList();
        }

        private static IList<Racing> ReadVirtualSportMatches(IDataReader reader)
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

        #endregion Binding
    }
}