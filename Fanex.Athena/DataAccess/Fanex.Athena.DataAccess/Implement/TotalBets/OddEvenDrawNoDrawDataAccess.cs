// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDrawDataAccess.cs" company="Nexcel Solution">
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
    /// Odd/Even/1X2/DrawNoDraw data access.
    /// </summary>
    public class OddEvenDrawNoDrawDataAccess : IOddEvenDrawNoDrawDataAccess
    {
        /// <summary>
        /// Get Odd/Even/1X2/DrawNoDraw data list.
        /// </summary>
        /// <param name="metaModel">Odd/Even/1X2/DrawNoDraw MetaModel value.</param>
        /// <returns>A list of Odd/Even/1X2/DrawNoDraw.</returns>
        public IList<OddEvenDrawNoDraw> GetOddEventDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel)
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

            using (var db = new ObjectDb(SpName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    return this.SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }
        }

        #region Utility

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Odd/Even/1X2/DrawNoDraw.</returns>
        private IList<OddEvenDrawNoDraw> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
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
                    LiveHomeScore = reader.ToString("livehomescore"),
                    LiveAwayScore = reader.ToString("liveawayscore"),
                    LiveIndicator = reader.ToInt("liveindicator"),
                    BetCountOddEven = reader.ToInt("betcount_oe"),
                    BetCountOneXTwo = reader.ToInt("betcount_1x2"),
                    BetCountDrawNoDraw = reader.ToInt("betcount_dnd"),
                    BetCountOddEven1StHalf = reader.ToInt("betcount_oe_1st"),
                    BetCountOneXTow1StHalf = reader.ToInt("betcount_1x2_1st"),
                    OddEvenFt = this.GetTotalScore(reader, oddEvenHomeColumn, oddEvenAwayColumn),
                    OneXTwoFt = this.GetOneXTwoScore(reader, oneXTwoOdd1, oneXTwoOddX, oneXTwoOdd2),
                    DrawNoDraw = this.GetTotalScore(reader, drawNoDrawHomeColumn, drawNoDrawAwayColumn),
                    OddEvenFirstHalf = this.GetTotalScore(reader, oddEvenHome1StHalfColumn, oddEvenAway1StHalfColumn),
                    OneXTwoFirstHalf = this.GetOneXTwoScore(reader, oneXTwo1StHalfOdd1, oneXTwo1StHalfOddX, oneXTwo1StHalfOdd2),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        /// <summary>
        /// Get total score.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="homeColumnName">Home column name value.</param>
        /// <param name="awayColumnName">Away column name value.</param>
        /// <returns>Total score.</returns>
        private decimal GetTotalScore(IDataReader reader, string homeColumnName, string awayColumnName)
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
        private decimal GetOneXTwoScore(IDataReader reader, string columnOne, string columnX, string columnTwo)
        {
            var onwScore = reader.ToDecimal(columnOne);
            var drawScore = reader.ToDecimal(columnX);
            var twoScore = reader.ToDecimal(columnTwo);
            return onwScore + drawScore + twoScore;
        }

        #endregion Utility
    }
}