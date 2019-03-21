// -----------------------------------------------------------------------
// <copyright file="ResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DataAccess;
    using Common.DataAccess.Extensions;
    using Common.DataAccess.Interfaces;
    using Common.Model.DTO;
    using Common.Utility.Services.Interfaces;
    using Fanex.Common.Extensions;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;

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

        public IList<Outright> GetOutRight(ResultMetaModel metal)
        {
            const string UspName = "Lrf_Result_Outright";
            var uspParams = new
            {
                fromdate = metal.FromDate,
                todate = metal.ToDate,
                isshowall = !_appSettings.IsProduction
            };

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

        public async Task<IList<Sport>> GetSports(DateTime dateValue, int licenseeId)
        {
            const string UspName = "Lrf_Result_SportSelByDate";

            using (var db = this.GetObjectDb(UspName))
            {
                var balanceUp = await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false);
                db.SetParameterValue("eventdate", dateValue);
                db.SetParameterValue("licenseeid", licenseeId);
                db.SetParameterValue("movebetsday", balanceUp.MoveBetsDay);
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

        public async Task<IList<League>> GetLeagues(DateTime dateValue, int sportId)
        {
            const string UspName = "Lrf_Result_LeagueSelByDate";
            var balanceUp = await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false);
            var uspParams = new
            {
                eventdate = dateValue,
                sporttype = sportId,
                isshowall = !_appSettings.IsProduction,
                movebetsday = balanceUp.MoveBetsDay
            };

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

        /// <summary>
        /// Get data for VS
        /// </summary>
        public IList<Racing> GetVirtualSportMatches(ResultMetaModel metalModel)
        {
            const string UspName = "Lrf_Result_RaceSelByMeeting";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId
            };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    return ReadVirtualSportMatches(reader);
                }
            }
        }

        public IList<T> GetPopUpMatches<T>(ResultPopupMetaModel metaModel)
        {
            const string UspName = "Lrf_BetList_SelMatchResult";
            var uspParams = new
            {
                matchid = metaModel.MatchId,
                raceno = metaModel.RaceNo.HasValue ? metaModel.RaceNo.Value.ToString(CultureInfo.InvariantCulture) : null,
                bettype = metaModel.Bettype,
                betid = metaModel.BetId
            };

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
        /// Get match.
        /// </summary>
        /// <typeparam name="T">Type of result.</typeparam>
        /// <param name="metalModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public async Task<IList<T>> GetMatches<T>(ResultMetaModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelByLeague";
            var balanceUp = await _balanceUpDataAccess.GetBalanceUp().ConfigureAwait(false);
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                sporttype = metalModel.SportId,
                leagueid = metalModel.LeagueId,
                movebetsday = balanceUp.MoveBetsDay
            };

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
                db.SetParameterValue("matchid", matchId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var result = BindSportsbook(reader);
                        result.GameDetails = GetGameDetails(reader).ToList();
                        return result;
                    }

                    return null;
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
        private static IList<Sportsbook> ReadSportbookMatches(IDataReader reader, ResultPopupMetaModel metaModel = null)
        {
            var results = new List<Sportsbook>();

            while (reader.Read())
            {
                var result = BindSportsbook(reader);

                if (metaModel != null)
                {
                    result.BetID = IsFastMarket(metaModel.Bettype) ? metaModel.Handicap1.ToString() : (metaModel.BetId ?? 0).ToString();
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

        private static bool IsFastMarket(int bettype)
        {
            const int FastMarketMinBetType = 221;
            const int FastMarketMaxBetType = 227;
            return bettype >= FastMarketMinBetType && bettype <= FastMarketMaxBetType;
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