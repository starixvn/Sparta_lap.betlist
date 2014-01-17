// -----------------------------------------------------------------------
// <copyright file="MoneyLineDataAccess.cs" company="Nexcel Solution">
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
    /// Money Line data access.
    /// </summary>
    public class MoneyLineDataAccess : IMoneyLineDataAccess
    {
        /// <summary>
        /// Get money line data list.
        /// </summary>
        /// <param name="metaModel">Money line MetaModel value.</param>
        /// <returns>A list of money line.</returns>
        public IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel2021";
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
        /// <returns>A list of Money line.</returns>
        private IList<MoneyLine> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<MoneyLine>();

            // Fulltime column name
            var totalHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds", isConvert, baseCurrency);
            var totalAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds", isConvert, baseCurrency);
            var homeScoreColumnName = DataHelper.GetCurrencyColumnName("winlost_home", isConvert, baseCurrency);
            var awayScoreColumnName = DataHelper.GetCurrencyColumnName("winlost_away", isConvert, baseCurrency);

            // First half column name
            var totalHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HT", isConvert, baseCurrency);
            var totalAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HT", isConvert, baseCurrency);
            var homeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("winlost_home_HT", isConvert, baseCurrency);
            var awayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("winlost_away_HT", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new MoneyLine
                {
                    ShowTime = reader.ToString("showtime"),
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    TotalHomeFt = reader.ToDecimal(totalHomeScoreColumnName),
                    TotalAwayFt = reader.ToDecimal(totalAwayScoreColumnName),
                    ForecastHomeFt = reader.ToDecimal(homeScoreColumnName),
                    ForecastAwayFt = reader.ToDecimal(awayScoreColumnName),
                    TotalHomeFirstHalf = reader.ToDecimal(totalHomeScore1StHalfColumnName),
                    TotalAwayFirstHalf = reader.ToDecimal(totalAwayScore1StHalfColumnName),
                    ForecastHomeFirstHalf = reader.ToDecimal(homeScore1StHalfColumnName),
                    ForecastAwayFirstHalf = reader.ToDecimal(awayScore1StHalfColumnName)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        #endregion Utility
    }
}