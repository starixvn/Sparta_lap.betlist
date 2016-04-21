// --------------------------------------------------------------------------
// <copyright file="ForecastDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Implement.Forecast
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Core.Extensions;
    using Data;
    using Fanex.Athena.Models.DTO.Forecast.ForecastHdou;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;
    using Fanex.Athena.Models.ViewModel.Forecast;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Interface.Forecast;
    using Model.Core.MetaModel;
    using Models.DTO.Forecast.Forecast1X2;
    using Models.DTO.Forecast.ForecastAH;
    using Models.MetaModel.Forecast;
    using Resources;
    using League = Fanex.Athena.Model.Core.DTO.Common.League;

    /// <summary>
    /// Forecast Data Access.
    /// </summary>
    public class ForecastDataAccess : IForecastDataAccess
    {
        #region Forecast AH

        /// <summary>
        /// Get Forecast top odds by match.
        /// </summary>
        /// <param name="matchId">The match's id..</param>
        /// <param name="betType">The bet type..</param>
        /// <returns>Forecast AH data..</returns>
        public ForecastAH GetForecastTopOddsByMatch(int matchId, int betType)
        {
            const string UspName = "Lrf_Forecast_OddsSelTopByMatch";

            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("matchid", DbType.Int32, matchId);
                db.AddInParameter("betType", DbType.Int32, betType);
                return db.Query<ForecastAH>().FirstOrDefault();
            }
        }

        /// <summary>
        /// Get Forecast Handicap.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>A table of forecast handicap.</returns>
        public DataTable GetForecastHandicap(ForecastAHMetaModel metaModel, int userId)
        {
            const string UspName = "Lrf_Forecast_SelHDP";
            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
                db.AddInParameter("betType", DbType.Int32, metaModel.BetType);
                var dataset = db.ExecuteDataSet();
                return dataset != null && dataset.Tables.Count > 0 ? dataset.Tables[0] : null;
            }
        }

        /// <summary>
        /// Get Forecast Over/Under.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>A table of forecast over under.</returns>
        public DataTable GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId)
        {
            const string UspName = "Lrf_Forecast_SelOU";

            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
                db.AddInParameter("betType", DbType.Int32, metaModel.BetType);
                var dataset = db.ExecuteDataSet();
                return dataset != null && dataset.Tables.Count > 0 ? dataset.Tables[0] : null;
            }
        }

        #endregion Forecast AH

        #region Forecast 1 x 2

        /// <summary>
        /// Get Forecast one x two's running league.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>A list of running league.</returns>
        public IList<League> GetForecastRunningLeague(ForecastBaseMetaModel metaModel)
        {
            const string UspName = "Lrf_LeagueSelRunning";
            var leagues = new List<League>();

            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("CurrencyId", DbType.Int32, 0);
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("betTypes", DbType.String, metaModel.BetTypeString);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leagues.Add(new League { LeagueId = reader.ToInt("leagueid") });
                    }
                }

                return leagues;
            }
        }

        /// <summary>
        /// Get Forecast one x two by league.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <returns>A list of forecast one x two by league.</returns>
        public IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            const string UspName = "Lrf_Forecast1x2_SelByLeague";

            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);

                using (var reader = db.ExecuteReader())
                {
                    return GetForecastOneXTwoList(metaModel, reader);
                }
            }
        }

        /// <summary>
        /// Get Forecast one x two by match.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>A list of forecast one x two by match..</returns>
        public IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId)
        {
            const string UspName = "Lrf_Forecast_Sel1X2";

            using (var db = new ObjectDb(UspName))
            {
                db.AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
                db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("matchid", DbType.Int32, metaModel.MatchId);
                db.AddInParameter("betType", DbType.Int32, metaModel.BetType);

                using (var reader = db.ExecuteReader())
                {
                    return GetForecastOneXTwoList(metaModel, reader);
                }
            }
        }

        #endregion Forecast 1 x 2

        #region Forecast HDP OU

        /// <summary>
        /// Get forecast handicap over under.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>List result.</returns>
        public IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel)
        {
            const string UspName = "Lrf_Forecast_SelByLeague";

            var paramValues = new
            {
                LicenseeId = metaModel.LicenseeId,
                UserId = metaModel.UserId,
                LicenseeType = metaModel.LicenseeType,
                CurrencyId = metaModel.CurrencyId,
                LeagueId = metaModel.LeagueId,
                Baseoffsethdp = 0,
                baseoffsetou = 0,
                baseoffseththdp = 0,
                baseoffsethtou = 0
            };
            using (var db = new ObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(paramValues))
                {
                    return GetForecastHandicapOverUnderList(reader, metaModel);
                }
            }
        }

        #endregion Forecast HDP OU

        #region New Forecast

        /// <summary>
        /// Get new forecast data.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <param name="matchInfo">Match information.</param>
        /// <returns>New forecast result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo matchInfo)
        {
            var forecastData = new NewForecastHdpOuData();
            using (var db = new ObjectDb("Lrf_Forecast_MatchTotalForecast"))
            {
                db.AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                db.AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
                db.AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
                db.AddOutParameter("HomeScore", DbType.Int32, 4);
                db.AddOutParameter("AwayScore", DbType.Int32, 4);
                db.AddOutParameter("MatchTime", DbType.DateTime, 4);
                using (var ds = db.ExecuteDataSet())
                {
                    if (ds.Tables.Count > 1)
                    {
                        var totalTable = ds.Tables[0];
                        var detailTable = ds.Tables[1];
                        foreach (DataRow dr in totalTable.Rows)
                        {
                            forecastData.TotalItemList.Add(new NewForecastHdpOuTotalItem
                            {
                                Type = Converter.ToInt(dr["Type"]),
                                TicketNo = Converter.ToInt(dr["TotalTickets"]),
                                TotalTurnoverHome = Converter.ToDecimal(dr["HomeTurnover" + metaModel.BaseCurrency]),
                                TotalTurnoverAway = Converter.ToDecimal(dr["AwayTurnover" + metaModel.BaseCurrency]),
                                TotalTurnoverOver = Converter.ToDecimal(dr["OverTurnover" + metaModel.BaseCurrency]),
                                TotalTurnoverUnder = Converter.ToDecimal(dr["UnderTurnover" + metaModel.BaseCurrency]),
                            });
                        }

                        foreach (DataRow dr in detailTable.Rows)
                        {
                            forecastData.DetailItemList.Add(new NewForecastHdpOuDetailItem
                            {
                                Bettype = Converter.ToInt(dr["Bettype"]),
                                Hdp = Converter.ToDecimal(dr["Hdp"]),
                                HomeWinLoss = Converter.ToDecimal(dr["HomeWinloss" + metaModel.BaseCurrency]),
                                AwayWinLoss = Converter.ToDecimal(dr["AwayWinloss" + metaModel.BaseCurrency]),
                            });
                        }
                    }

                    var date = db.GetParameterValue("MatchTime").ToString();
                    matchInfo.HomeScore = Converter.ToInt(db.GetParameterValue("HomeScore"));
                    matchInfo.AwayScore = Converter.ToInt(db.GetParameterValue("AwayScore"));
                    matchInfo.KickOffDateString = string.IsNullOrEmpty(date)
                        ? ViewResources.Live + "!"
                        : Converter.ToDateTime(date).ToString("MM/dd/yyyy hh:mm tt");
                }
            }

            return forecastData;
        }

        #endregion New Forecast

        /// <summary>
        /// Get forecast list.
        /// </summary>
        /// <param name="metaModel">The parameter.</param>
        /// <param name="reader">Data reader.</param>
        /// <returns>A list of forecast one x two.</returns>
        private static IList<ForecastOneXTwo> GetForecastOneXTwoList(BaseMetaModel metaModel, IDataReader reader)
        {
            var forecasts = new List<ForecastOneXTwo>();
            var extraColumnName = "isExtra";
            var homeWinLossColumn = DataHelper.GetCurrencyColumnName("hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var drawWinLossColumn = DataHelper.GetCurrencyColumnName("dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var awayWinLossColumn = DataHelper.GetCurrencyColumnName("al", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeHomeWinLossColumn = DataHelper.GetCurrencyColumnName("ht_hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeDrawWinLossColumn = DataHelper.GetCurrencyColumnName("ht_dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeAwayWinLossColumn = DataHelper.GetCurrencyColumnName("ht_al", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraHomeWinLossColumn = DataHelper.GetCurrencyColumnName("x_hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraDrawWinLossColumn = DataHelper.GetCurrencyColumnName("x_dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraAwayWinLossColumn = DataHelper.GetCurrencyColumnName("x_al", metaModel.IsConvert, metaModel.BaseCurrency);
            var hasExtraColumn = reader.HasColumn(extraColumnName);
            while (reader.Read())
            {
                var item = new ForecastOneXTwo
                {
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid"),
                    LiveIndicator = reader.ToBool("liveindicator"),
                    LiveHomeScore = reader.ToByte("livehomescore"),
                    LiveAwayScore = reader.ToByte("liveawayscore"),
                    ShowTime = reader.ToString("showtime"),
                    IsNeutral = reader.ToBool("isNeutral")
                };

                if (hasExtraColumn == false || reader.ToInt(extraColumnName) == 0)
                {
                    item.IsExtra = false;
                    item.HomeWinLoss = reader.ToDecimal(homeWinLossColumn);
                    item.DrawWinLoss = reader.ToDecimal(drawWinLossColumn);
                    item.AwayWinLoss = reader.ToDecimal(awayWinLossColumn);
                    item.HalfTimeHomeWinLoss = reader.ToDecimal(halfTimeHomeWinLossColumn);
                    item.HalfTImeDrawWinLoss = reader.ToDecimal(halfTimeDrawWinLossColumn);
                    item.HalfTimeAwayWinLoss = reader.ToDecimal(halfTimeAwayWinLossColumn);
                }
                else
                {
                    item.IsExtra = true;
                    item.HalfTimeHomeWinLoss = reader.ToDecimal(extraHomeWinLossColumn);
                    item.HalfTImeDrawWinLoss = reader.ToDecimal(extraDrawWinLossColumn);
                    item.HalfTimeAwayWinLoss = reader.ToDecimal(extraAwayWinLossColumn);
                }

                forecasts.Add(item);
            }

            return forecasts;
        }

        /// <summary>
        /// Get forecast handicap over under list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>List result.</returns>
        private static IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnderList(IDataReader reader, ForecastHandicapOverUnderMetaModel metaModel)
        {
            var forecasts = new List<ForecastHandicapOverUnder>();

            // Full time column name
            var winLossHdpMinus1 = DataHelper.GetCurrencyColumnName("WinLost_M1_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossHdp = DataHelper.GetCurrencyColumnName("WinLost_0_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossHdpPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOuPlus2 = DataHelper.GetCurrencyColumnName("WinLost_2_OU", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOuPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_OU", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOu = DataHelper.GetCurrencyColumnName("WinLost_0_OU", metaModel.IsConvert, metaModel.BaseCurrency);

            // Half time column name
            var halfTimeWinLossHdpMinus1 = DataHelper.GetCurrencyColumnName("WinLost_M1_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossHdp = DataHelper.GetCurrencyColumnName("WinLost_0_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossHdpPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOuPlus2 = DataHelper.GetCurrencyColumnName("WinLost_2_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOuPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOu = DataHelper.GetCurrencyColumnName("WinLost_0_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);

            while (reader.Read())
            {
                var item = new ForecastHandicapOverUnder
                {
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid"),
                    LiveIndicator = reader.ToBool("liveindicator"),
                    LiveHomeScore = reader.ToByte("livehomescore"),
                    LiveAwayScore = reader.ToByte("liveawayscore"),
                    ShowTime = reader.ToString("showtime"),
                    IsNeutral = reader.ToBool("isNeutral"),
                    HandicapMinus1 = reader.ToDecimal(winLossHdpMinus1),
                    Handicap = reader.ToDecimal(winLossHdp),
                    HandicapPlus1 = reader.ToDecimal(winLossHdpPlus1),
                    OverUnderPlus2 = reader.ToDecimal(winLossOuPlus2),
                    OverUnderPlus1 = reader.ToDecimal(winLossOuPlus1),
                    OverUnder = reader.ToDecimal(winLossOu),
                    HtHandicapMinus1 = reader.ToDecimal(halfTimeWinLossHdpMinus1),
                    HtHandicap = reader.ToDecimal(halfTimeWinLossHdp),
                    HtHandicapPlus1 = reader.ToDecimal(halfTimeWinLossHdpPlus1),
                    HtOverUnderPlus2 = reader.ToDecimal(halfTimeWinLossOuPlus2),
                    HtOverUnderPlus1 = reader.ToDecimal(halfTimeWinLossOuPlus1),
                    HtOverUnder = reader.ToDecimal(halfTimeWinLossOu)
                };
                CalculateOffsetHandicapOverUnder(ref item);
                forecasts.Add(item);
            }

            return forecasts;
        }

        /// <summary>
        /// Calculate offset.
        /// </summary>
        /// <param name="forecast">Parameter value.</param>
        private static void CalculateOffsetHandicapOverUnder(ref ForecastHandicapOverUnder forecast)
        {
            var liveHomeScore = forecast.LiveHomeScore;
            var liveAwayScore = forecast.LiveAwayScore;

            var offsetHdp = liveHomeScore - liveAwayScore;
            var offsetOu = liveHomeScore + liveAwayScore;

            forecast.OffsetHdpMinus1 = offsetHdp - 1;
            forecast.OffsetHdp = offsetHdp;
            forecast.OffsetHdpPlus1 = offsetHdp + 1;
            forecast.OffsetOuPlus2 = offsetOu + 2;
            forecast.OffsetOuPlus1 = offsetOu + 1;
            forecast.OffsetOu = offsetOu;
        }
    }
}