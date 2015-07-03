// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.TotalBets
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using DataAccess.Interface.TotalBets;
    using Extension;
    using Fanex.Common.Extensions;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Handicap Over Under data access.
    /// </summary>
    public class HandicapOverUnderDataAccess : IHandicapOverUnderDataAccess
    {
        /// <summary>
        /// Get Handicap Over/Under data list.
        /// </summary>
        /// <param name="metaModel">Handicap Over/Under MetaModel value.</param>
        /// <param name="totalData">HandicapOverUnder value.</param>
        /// <returns>A list of Handicap Over/Under.</returns>
        public IList<HandicapOverUnder> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel, out HandicapOverUnder totalData)
        {
            const string SpName = "Lrf_TotalBets_Sel1378";
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                LicenseeType = metaModel.LicenseeType,
                CurrencyId = metaModel.CurrencyId,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                UserId = metaModel.UserId
            };

            IList<HandicapOverUnder> data;

            using (var db = new ObjectDb(SpName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    data = SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    reader.NextResult();
                    totalData = ReadTotalData(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }

            return data;
        }

        #region Utility

        /// <summary>
        /// Get Total Score.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="homeColumnName">Home column name value.</param>
        /// <param name="awayColumnName">Away column name value.</param>
        /// <returns>Total Score.</returns>
        private static decimal GetTotalScore(IDataReader reader, string homeColumnName, string awayColumnName)
        {
            var homeScore = reader.ToDecimal(homeColumnName);
            var awayScore = reader.ToDecimal(awayColumnName);

            return homeScore - awayScore;
        }

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Handicap Over/Under.</returns>
        private static IList<HandicapOverUnder> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<HandicapOverUnder>();

            // Fulltime column name
            var hdpHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds_HDP", isConvert, baseCurrency);
            var hdpAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds_HDP", isConvert, baseCurrency);
            var overUnderHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds_OU", isConvert, baseCurrency);
            var overUnderAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds_OU", isConvert, baseCurrency);

            // First half column name
            var hdpHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HTHDP", isConvert, baseCurrency);
            var hdpAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HTHDP", isConvert, baseCurrency);
            var overUnderHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HTOU", isConvert, baseCurrency);
            var overUnderAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HTOU", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new HandicapOverUnder
                {
                    ShowTime = reader.ToString("showtime"),
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    LiveHomeScore = reader.ToString("livehomescore"),
                    LiveAwayScore = reader.ToString("liveawayscore"),
                    LiveIndicator = reader.ToInt("liveindicator"),
                    BetCountHandicap = reader.ToInt("betcount_hdp"),
                    BetCountOverUnder = reader.ToInt("betcount_ou"),
                    BetCountHandicap1StHalf = reader.ToInt("BetCount_HTHDP"),
                    BetCountOverUnder1StHalf = reader.ToInt("BetCount_HTOU"),
                    FullTimeHandicap = GetTotalScore(reader, hdpHomeScoreColumnName, hdpAwayScoreColumnName),
                    FullTimeOverUnder = GetTotalScore(reader, overUnderHomeScoreColumnName, overUnderAwayScoreColumnName),
                    FirstTimeHandicap = GetTotalScore(reader, hdpHomeScore1StHalfColumnName, hdpAwayScore1StHalfColumnName),
                    FirstTimeOverUnder = GetTotalScore(reader, overUnderHomeScore1StHalfColumnName, overUnderAwayScore1StHalfColumnName),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        /// <summary>
        /// Read total data.
        /// </summary>
        /// <param name="reader">IDataReader class.</param>
        /// <param name="isConvert">Boolean isConvert.</param>
        /// <param name="baseCurrency">Base currency.</param>
        /// <returns>HandicapOverUnder data.</returns>
        private static HandicapOverUnder ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            HandicapOverUnder totalData = null;

            if (reader.Read())
            {
                // Fulltime column name
                var hdpHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds_HDP", isConvert, baseCurrency);
                var hdpAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds_HDP", isConvert, baseCurrency);
                var overUnderHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds_OU", isConvert, baseCurrency);
                var overUnderAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds_OU", isConvert, baseCurrency);

                // First half column name
                var hdpHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HTHDP", isConvert, baseCurrency);
                var hdpAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HTHDP", isConvert, baseCurrency);
                var overUnderHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HTOU", isConvert, baseCurrency);
                var overUnderAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HTOU", isConvert, baseCurrency);

                totalData = new HandicapOverUnder
                {
                    FullTimeHandicap = GetRunningStake(reader, hdpHomeScoreColumnName, hdpAwayScoreColumnName),
                    FullTimeOverUnder = GetRunningStake(reader, overUnderHomeScoreColumnName, overUnderAwayScoreColumnName),
                    FirstTimeHandicap = GetRunningStake(reader, hdpHomeScore1StHalfColumnName, hdpAwayScore1StHalfColumnName),
                    FirstTimeOverUnder = GetRunningStake(reader, overUnderHomeScore1StHalfColumnName, overUnderAwayScore1StHalfColumnName)
                };
            }

            return totalData ?? new HandicapOverUnder();
        }

        private static decimal GetRunningStake(IDataReader reader, string homeColumnName, string awayColumnName)
        {
            var homeScore = reader.ToDecimal(homeColumnName);
            var awayScore = reader.ToDecimal(awayColumnName);

            return homeScore + awayScore;
        }

        #endregion Utility
    }
}