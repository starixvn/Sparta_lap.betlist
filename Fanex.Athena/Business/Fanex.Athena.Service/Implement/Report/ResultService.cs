// -----------------------------------------------------------------------
// <copyright file="ResultService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Service.Interface.Report;
    using Fanex.Result.Core.DTO;

    /// <summary>
    /// Result service class.
    /// </summary>
    public class ResultService : IResultService
    {
        private readonly IResultDataAccess _dataAccess;

        public ResultService()
        {
            this._dataAccess = new ResultDataAccess();
        }

        /// <summary>
        /// Get out right data.
        /// </summary>
        /// <param name="metal">The metal model.</param>
        /// <returns>The out right data list.</returns>
        public IList<OutRight> GetOutRight(Models.MetaModel.Report.ResultMetalModel metal)
        {
            var data = this._dataAccess.GetOutRight(metal);
            if (data != null && data.Rows.Count > 0)
            {
                return
                    data.AsEnumerable()
                        .Select(
                            i =>
                                new OutRight
                                {
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    SportType = int.Parse(i["sporttype"].ToString()),
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    TeamId = int.Parse(i["teamid"].ToString()),
                                    Status = i["status"].ToString()
                                })
                        .ToList();
            }

            return new List<OutRight>();
        }

        public IList<Models.DTO.Common.Sport> GetSports(DateTime date, int licenseeId)
        {
            return this._dataAccess.GetSports(date, licenseeId);
        }

        public IList<Models.DTO.Common.League> GetLeagues(DateTime date, int sportId)
        {
            return this._dataAccess.GetLeagues(date, sportId);
        }

        #region Report

        public IList<Sportbook> GetMatchs(Models.MetaModel.Report.ResultMetalModel metalModel)
        {
            var data = this._dataAccess.GetMatchs(metalModel);
            return BindSportbook(data);
        }

        public IList<NumberGame> GetNumberGameMatchs(Models.MetaModel.Report.ResultMetalModel metalModel)
        {
            var dataNg = this._dataAccess.GetNumberGameMatchs(metalModel);
            return BindingNumberGame(dataNg);
        }

        public IList<Racing> GetRacingMatch(Models.MetaModel.Report.ResultMetalModel metalModel)
        {
            var dataRc = this._dataAccess.GetRacingMatch(metalModel);
            return BidingRacing(dataRc);
        }

        public IList<LiveCasino> GetLiveCasinoMatchs(Models.MetaModel.Report.ResultMetalModel metalModel)
        {
            var dataLc = this._dataAccess.GetMatchs(metalModel);
            return BidingLiveCasinos(dataLc);
        }

        #endregion Report

        #region Popup

        public IList<Sportbook> GetPopUpMatch(int matchId, int licenseeId, int userId)
        {
            var data = this._dataAccess.GetPopUpMatch(matchId, -1, licenseeId, userId);
            return BindSportbook(data);
        }

        /// <summary>
        /// Get parlay match for pop up page.
        /// </summary>
        /// <param name="refNo">The reference no.</param>
        /// <returns>The parlay data list.</returns>
        [SuppressMessage(
           "StyleCop.CSharp.Nexcel.NexcelCustomRules",
           "SP2101:MethodMustNotContainMoreLinesThan",
           Justification = "Reviewed. Suppression is OK here.")]
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        public IList<Parlay> GetPopUpParlayMatch(long refNo)
        {
            var data = this._dataAccess.GetPopUpParalayMatch(refNo);
            if (data != null && data.Rows.Count > 0)
            {
                var dataList = data.AsEnumerable();
                var dataParlay = new List<Parlay>();

                foreach (var row in dataList)
                {
                    var leagueId = int.Parse(row["LeagueID"].ToString());
                    if (dataParlay.All(i => i.LeagueId != leagueId))
                    {
                        dataParlay.Add(
                            new Parlay
                            {
                                LeagueId = leagueId,
                                SportId = int.Parse(row["sporttype"].ToString()),
                                Sportbook =
                                    dataList.Where(i => i["LeagueID"].ToString() == leagueId.ToString(CultureInfo.InvariantCulture))
                                        .Select(
                                            i =>
                                                new Sportbook
                                                {
                                                    AwayId = int.Parse(i["awayID"].ToString()),
                                                    HomeId = int.Parse(i["homeID"].ToString()),
                                                    BetStatus =
                                                        i["betstatus"] != DBNull.Value
                                                            ? i["betstatus"].ToString()
                                                            : string.Empty,
                                                    LeagueId = leagueId,
                                                    MatchId = int.Parse(i["matchid"].ToString()),
                                                    EventStatus = i["eventstatus"].ToString(),
                                                    FinalAwayScore =
                                                        i["finalawayscore"] != DBNull.Value
                                                            ? int.Parse(i["finalawayscore"].ToString())
                                                            : new Nullable<int>(),
                                                    HtAwayScore =
                                                        i["htawayscore"] != DBNull.Value
                                                            ? int.Parse(i["htawayscore"].ToString())
                                                            : new Nullable<int>(),
                                                    Color = i.Table.Columns.Contains("lcolor") &&
                                                        i["lcolor"] != DBNull.Value
                                                            ? i["lcolor"].ToString()
                                                            : string.Empty,
                                                    FinalHomeScore =
                                                        i["finalhomescore"] != DBNull.Value
                                                            ? int.Parse(i["finalhomescore"].ToString())
                                                            : new Nullable<int>(),
                                                    HtHomeScore =
                                                        i["finalhomescore"] != DBNull.Value
                                                            ? int.Parse(i["hthomescore"].ToString())
                                                            : new Nullable<int>(),
                                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                                    IsNeutral = bool.Parse(i["IsNeutral"].ToString()),
                                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                                    MatchCode = i["matchcode"].ToString(),
                                                    GoalRecord = i["goalrecord"].ToString(),
                                                    ResultValue = string.Empty,
                                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                                    RefNo = refNo.ToString(CultureInfo.InvariantCulture),
                                                })
                                        .ToList()
                            });
                    }
                }

                return dataParlay;
            }

            return new List<Parlay>();
        }

        public IList<NumberGame> GetPopUpNumberGameMatchs(int matchId)
        {
            var data = this._dataAccess.GetPopUpNumberGameMatch(matchId);
            return BindingNumberGame(data);
        }

        public IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int licenseeId, int userId)
        {
            var dataLc = this._dataAccess.GetPopUpMatch(matchId, -1, licenseeId, userId);
            return BidingLiveCasinos(dataLc);
        }

        /// <summary>
        /// Get racing data for popup page.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="raceNo">The reference number.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The racing data list.</returns>
        public IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int licenseeId, int userId)
        {
            var dataRc = this._dataAccess.GetPopUpMatch(matchId, raceNo, licenseeId, userId);
            return BidingRacing(dataRc);
        }

        #endregion Popup

        #region Biding

        /// <summary>
        /// Mapping data to Sport-book DTO.
        /// </summary>
        /// <param name="data">The data table.</param>
        /// <returns>The sport-book data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        private static IList<Sportbook> BindSportbook(DataTable data)
        {
            if (data != null && data.Rows.Count > 0)
            {
                return
                    data.AsEnumerable()
                        .Select(
                            i =>
                                new Sportbook
                                {
                                    AwayId = int.Parse(i["awayID"].ToString()),
                                    HomeId = int.Parse(i["homeID"].ToString()),
                                    BetStatus =
                                        i["betstatus"] != DBNull.Value ? i["betstatus"].ToString() : string.Empty,
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    MatchId = int.Parse(i["matchid"].ToString()),
                                    EventStatus = i["eventstatus"].ToString(),
                                    FinalAwayScore =
                                        i["finalawayscore"] != DBNull.Value
                                            ? int.Parse(i["finalawayscore"].ToString())
                                            : new Nullable<int>(),
                                    HtAwayScore =
                                        i["htawayscore"] != DBNull.Value ? int.Parse(i["htawayscore"].ToString()) : new Nullable<int>(),
                                    Color = i.Table.Columns.Contains("lcolor") && i["lcolor"] != DBNull.Value ? i["lcolor"].ToString() : string.Empty,
                                    FinalHomeScore =
                                        i["finalhomescore"] != DBNull.Value
                                            ? int.Parse(i["finalhomescore"].ToString())
                                            : new Nullable<int>(),
                                    HtHomeScore =
                                        i["hthomescore"] != DBNull.Value
                                            ? int.Parse(i["hthomescore"].ToString())
                                            : new Nullable<int>(),
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    IsNeutral = bool.Parse(i["IsNeutral"].ToString()),
                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                    MatchCode = i["matchcode"].ToString(),
                                    GoalRecord = i["goalrecord"].ToString(),
                                    ResultValue = string.Empty,
                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                    RefNo = string.Empty
                                })
                        .ToList();
            }

            return new List<Sportbook>();
        }

        /// <summary>
        /// Mapping data to NG DTO.
        /// </summary>
        /// <param name="dataNg">The data table.</param>
        /// <returns>The NG data list.</returns>
        private static IList<NumberGame> BindingNumberGame(DataTable dataNg)
        {
            if (dataNg != null && dataNg.Rows.Count > 0)
            {
                dataNg.AsEnumerable().OrderBy(i => i["leagueId"]).OrderBy(i => i["matchid"]).OrderBy(i => i["seq"]);
                var dataListNg = new List<NumberGame>();
                foreach (var mtc in dataNg.AsEnumerable())
                {
                    var matchId = int.Parse(mtc["matchid"].ToString());
                    var existMatch = dataListNg.SingleOrDefault(i => i.MatchId == matchId);
                    if (existMatch != null)
                    {
                        existMatch.Seg.Add(int.Parse(mtc["seq"].ToString()));
                        existMatch.Ball.Add(int.Parse(mtc["number"].ToString()));
                    }
                    else
                    {
                        dataListNg.Add(
                            new NumberGame
                            {
                                LeagueId = int.Parse(mtc["leagueid"].ToString()),
                                MatchId = int.Parse(mtc["matchid"].ToString()),
                                EventStatus = mtc["eventstatus"].ToString(),
                                EventDate = DateTime.Parse(mtc["eventdate"].ToString()),
                                IsPending = mtc.Table.Columns.Contains("isrefund") && mtc["isrefund"].ToString() != "0",
                                KickOffTime = DateTime.Parse(mtc["kickofftime"].ToString()),
                                MatchCode = mtc["matchcode"].ToString(),
                                Seg = new List<int> { int.Parse(mtc["seq"].ToString()) },
                                Ball = new List<int> { int.Parse(mtc["number"].ToString()) }
                            });
                    }
                }

                return dataListNg;
            }

            return new List<NumberGame>();
        }

        /// <summary>
        /// Mapping data to Live casino DTO.
        /// </summary>
        /// <param name="dataLc">The data table.</param>
        /// <returns>The live casino data list.</returns>
        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1125:UseShorthandForNullableTypes", Justification = "Reviewed.")]
        private static IList<LiveCasino> BidingLiveCasinos(DataTable dataLc)
        {
            if (dataLc != null && dataLc.Rows.Count > 0)
            {
                return
                    dataLc.AsEnumerable()
                        .Select(
                            i =>
                                new LiveCasino
                                {
                                    ShowTime = i["showtime"].ToString(),
                                    LeagueId = int.Parse(i["leagueid"].ToString()),
                                    EventStatus = i["eventstatus"].ToString(),
                                    FinalAwayScore =
                                        i["finalawayscore"] != DBNull.Value ? int.Parse(i["finalawayscore"].ToString()) : new Nullable<int>(),
                                    FinalHomeScore =
                                        i["finalhomescore"] != DBNull.Value ? int.Parse(i["finalhomescore"].ToString()) : new Nullable<int>(),
                                    EventDate = DateTime.Parse(i["eventdate"].ToString()),
                                    IsPending = bool.Parse(i["IsPending"].ToString()),
                                    KickOffTime = DateTime.Parse(i["kickofftime"].ToString()),
                                    IsHTCal = Convert.ToBoolean(i["IsHTCal"]),
                                    Calculate = Convert.ToBoolean(i["Calculate"]),
                                    BetStatus = i["betstatus"] != DBNull.Value ? i["betstatus"].ToString() : string.Empty,
                                    Bettype = int.Parse(i["homeid2"].ToString())
                                })
                        .ToList();
            }

            return new List<LiveCasino>();
        }

        /// <summary>
        /// Mapping racing data from data table.
        /// </summary>
        /// <param name="dataRc">The data table.</param>
        /// <returns>The racing data list.</returns>
        private static IList<Racing> BidingRacing(DataTable dataRc)
        {
            if (dataRc != null && dataRc.Rows.Count > 0)
            {
                return
                    dataRc.AsEnumerable()
                        .Select(
                            i =>
                                new Racing
                                {
                                    LeagueId = int.Parse(i["meetingid"].ToString()),
                                    EngineId = i["HorseId"].ToString(),
                                    EngineNo = i["HorseNo"].ToString(),
                                    IsFavor = i["IsFavor"] != DBNull.Value && i["IsFavor"].ToString().Equals("1") ? true : false,
                                    IsSCR = i["IsSCR"] != DBNull.Value && i["IsSCR"].ToString().Equals("1") ? true : false,
                                    Place = int.Parse(i["place"].ToString()),
                                    PlaceOdds = decimal.Parse(i["placeodds"].ToString()),
                                    WinOdds = decimal.Parse(i["odds"].ToString()),
                                    RaceName = i["racename"] != DBNull.Value ? int.Parse(i["racename"].ToString()) : 0,
                                    KickOffTime = i["kickofftime"] != DBNull.Value ? DateTime.Parse(i["kickofftime"].ToString()) : DateTime.MinValue,
                                    Favourite = i["IsFavor"] != DBNull.Value ? i["IsFavor"].ToString() : string.Empty,
                                    RowCount = int.Parse(i["rowcount"].ToString()),
                                    SCROfMatch = i["SCR"].ToString(),
                                    TFOfMatch = i["TF"].ToString()
                                })
                        .ToList();
            }

            return new List<Racing>();
        }

        #endregion Biding
    }
}