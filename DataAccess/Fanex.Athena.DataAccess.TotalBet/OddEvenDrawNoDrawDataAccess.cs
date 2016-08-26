// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Extensions;
    using Data;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Common.Extensions;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Odd/Even/1X2/DrawNoDraw data access.
    /// </summary>
    public class OddEvenDrawNoDrawDataAccess : BaseDataAccess, IOddEvenDrawNoDrawDataAccess
    {
        /// <summary>
        /// GetOddEventDrawNoDrawDataList data.
        /// </summary>
        /// <param name="metaModel">Odd Even 1X2 DrawNoDraw MetaModel value.</param>
        /// <param name="totalData">OddEvenDrawNoDraw value.</param>
        /// <returns>A list of Odd/Even/1X2/DrawNoDraw.</returns>
        public IList<OddEvenDrawNoDraw> GetOddEventDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel, out OddEvenDrawNoDraw totalData)
        {
            const string SpName = "Lrf_TotalBets_Sel25";
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                LicenseeType = metaModel.LicenseeType,
                CurrencyId = metaModel.CurrencyId,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                TotalRows = metaModel.TotalRows,
                UserId = metaModel.UserId
            };

            IList<OddEvenDrawNoDraw> data;
            using (var db = this.GetObjectDb(SpName))
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
        /// Get total score.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="homeColumnName">Home column name value.</param>
        /// <param name="awayColumnName">Away column name value.</param>
        /// <returns>Total score.</returns>
        private static decimal GetTotalScore(IDataReader reader, string homeColumnName, string awayColumnName)
        {
            var homeScore = reader.ToDecimal(homeColumnName);
            var awayScore = reader.ToDecimal(awayColumnName);

            return homeScore - awayScore;
        }

        /// <summary>
        /// Get OneXTwo Score.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="columnOne">Column One value.</param>
        /// <param name="columnX">Column X value.</param>
        /// <param name="columnTwo">Column Two value.</param>
        /// <returns>One X Two score.</returns>
        private static decimal GetOneXTwoScore(IDataReader reader, string columnOne, string columnX, string columnTwo)
        {
            var onwScore = reader.ToDecimal(columnOne);
            var drawScore = reader.ToDecimal(columnX);
            var twoScore = reader.ToDecimal(columnTwo);
            return onwScore + drawScore + twoScore;
        }

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Odd/Even/1X2/DrawNoDraw.</returns>
        private static IList<OddEvenDrawNoDraw> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<OddEvenDrawNoDraw>();

            // Fulltime column name
            var oddEvenHomeColumn = DataHelper.GetCurrencyColumnName("home_odds_OE", isConvert, baseCurrency);
            var oddEvenAwayColumn = DataHelper.GetCurrencyColumnName("away_odds_OE", isConvert, baseCurrency);
            var oneXTwoOdd1 = DataHelper.GetCurrencyColumnName("odds_1", isConvert, baseCurrency);
            var oneXTwoOddX = DataHelper.GetCurrencyColumnName("odds_x", isConvert, baseCurrency);
            var oneXTwoOdd2 = DataHelper.GetCurrencyColumnName("odds_2", isConvert, baseCurrency);
            var drawNoDrawHomeColumn = DataHelper.GetCurrencyColumnName("home_odds_DND", isConvert, baseCurrency);
            var drawNoDrawAwayColumn = DataHelper.GetCurrencyColumnName("away_odds_DND", isConvert, baseCurrency);

            // First half column name
            var oddEvenHome1StHalfColumn = DataHelper.GetCurrencyColumnName("home_odds_OE_1st", isConvert, baseCurrency);
            var oddEvenAway1StHalfColumn = DataHelper.GetCurrencyColumnName("away_odds_OE_1st", isConvert, baseCurrency);
            var oneXTwo1StHalfOdd1 = DataHelper.GetCurrencyColumnName("odds_1_1st", isConvert, baseCurrency);
            var oneXTwo1StHalfOddX = DataHelper.GetCurrencyColumnName("odds_x_1st", isConvert, baseCurrency);
            var oneXTwo1StHalfOdd2 = DataHelper.GetCurrencyColumnName("odds_2_1st", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new OddEvenDrawNoDraw
                {
                    ShowTime = reader.ToString("showtime"),
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    Bettype = reader.ToInt("bettype"),
                    LiveHomeScore = reader.ToString("livehomescore"),
                    LiveAwayScore = reader.ToString("liveawayscore"),
                    LiveIndicator = reader.ToInt("liveindicator"),
                    BetCountOddEven = reader.ToInt("betcount_oe"),
                    BetCountOneXTwo = reader.ToInt("betcount_1x2"),
                    BetCountDrawNoDraw = reader.ToInt("betcount_dnd"),
                    BetCountOddEven1StHalf = reader.ToInt("betcount_oe_1st"),
                    BetCountOneXTow1StHalf = reader.ToInt("betcount_1x2_1st"),
                    OddEvenFt = GetTotalScore(reader, oddEvenHomeColumn, oddEvenAwayColumn),
                    OneXTwoFt = GetOneXTwoScore(reader, oneXTwoOdd1, oneXTwoOddX, oneXTwoOdd2),
                    DrawNoDraw = GetTotalScore(reader, drawNoDrawHomeColumn, drawNoDrawAwayColumn),
                    OddEvenFirstHalf = GetTotalScore(reader, oddEvenHome1StHalfColumn, oddEvenAway1StHalfColumn),
                    OneXTwoFirstHalf = GetOneXTwoScore(reader, oneXTwo1StHalfOdd1, oneXTwo1StHalfOddX, oneXTwo1StHalfOdd2),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        /// <summary>
        /// Get OddEvenDrawNoDraw Score.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>Odd Even Draw No Draw score.</returns>
        private static OddEvenDrawNoDraw ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (reader.Read())
            {
                // Fulltime column name
                var oddEvenHomeColumn = DataHelper.GetCurrencyColumnName("home_odds_OE", isConvert, baseCurrency);
                var oddEvenAwayColumn = DataHelper.GetCurrencyColumnName("away_odds_OE", isConvert, baseCurrency);
                var oneXTwoOdd1 = DataHelper.GetCurrencyColumnName("odds_1", isConvert, baseCurrency);
                var oneXTwoOddX = DataHelper.GetCurrencyColumnName("odds_x", isConvert, baseCurrency);
                var oneXTwoOdd2 = DataHelper.GetCurrencyColumnName("odds_2", isConvert, baseCurrency);
                var drawNoDrawHomeColumn = DataHelper.GetCurrencyColumnName("home_odds_DND", isConvert, baseCurrency);
                var drawNoDrawAwayColumn = DataHelper.GetCurrencyColumnName("away_odds_DND", isConvert, baseCurrency);

                // First half column name
                var oddEvenHome1StHalfColumn = DataHelper.GetCurrencyColumnName("home_odds_OE_1st", isConvert, baseCurrency);
                var oddEvenAway1StHalfColumn = DataHelper.GetCurrencyColumnName("away_odds_OE_1st", isConvert, baseCurrency);
                var oneXTwo1StHalfOdd1 = DataHelper.GetCurrencyColumnName("odds_1_1st", isConvert, baseCurrency);
                var oneXTwo1StHalfOddX = DataHelper.GetCurrencyColumnName("odds_x_1st", isConvert, baseCurrency);
                var oneXTwo1StHalfOdd2 = DataHelper.GetCurrencyColumnName("odds_2_1st", isConvert, baseCurrency);

                return new OddEvenDrawNoDraw
                {
                    OddEvenFt = GetRunningStake(reader, oddEvenHomeColumn, oddEvenAwayColumn),
                    OneXTwoFt = GetOneXTwoScore(reader, oneXTwoOdd1, oneXTwoOddX, oneXTwoOdd2),
                    DrawNoDraw = GetRunningStake(reader, drawNoDrawHomeColumn, drawNoDrawAwayColumn),
                    OddEvenFirstHalf = GetRunningStake(reader, oddEvenHome1StHalfColumn, oddEvenAway1StHalfColumn),
                    OneXTwoFirstHalf = GetOneXTwoScore(reader, oneXTwo1StHalfOdd1, oneXTwo1StHalfOddX, oneXTwo1StHalfOdd2),
                };
            }

            return new OddEvenDrawNoDraw();
        }

        private static decimal GetRunningStake(IDataReader reader, string columnOne, string columnTwo)
        {
            var onwScore = reader.ToDecimal(columnOne);
            var twoScore = reader.ToDecimal(columnTwo);
            return onwScore + twoScore;
        }

        #endregion Utility
    }
}