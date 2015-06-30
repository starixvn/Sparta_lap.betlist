// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetDataAccess.cs" company="Nexcel Solution">
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
    /// Home/Draw/Away no bet data access.
    /// </summary>
    public class HomeDrawAwayNoBetDataAccess : IHomeDrawAwayNoBetDataAccess
    {
        /// <summary>
        /// Get Home/Draw/Away no bet data list.
        /// </summary>
        /// <param name="metaModel">Home/Draw/Away no bet MetaModel value.</param>
        /// <returns>A list of Home/Draw/Away not bet.</returns>
        public IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel121";
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
                    return GetData(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }
        }

        #region Utility

        /// <summary>
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is convert value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>A list of Home/Draw/Away no bet.</returns>
        private static IList<HomeDrawAwayNoBet> GetData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<HomeDrawAwayNoBet>();

            var homOddsHnbColumn = DataHelper.GetCurrencyColumnName("home_odds_hnb", isConvert, baseCurrency);
            var awayOddsHnbColumn = DataHelper.GetCurrencyColumnName("away_odds_hnb", isConvert, baseCurrency);
            var homeOddsDnbColumn = DataHelper.GetCurrencyColumnName("home_odds_dnb", isConvert, baseCurrency);
            var awayOddsDnbColumn = DataHelper.GetCurrencyColumnName("away_odds_dnb", isConvert, baseCurrency);
            var homeOddsAnbColumn = DataHelper.GetCurrencyColumnName("home_odds_anb", isConvert, baseCurrency);
            var awayOddsAnbColumn = DataHelper.GetCurrencyColumnName("away_odds_anb", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new HomeDrawAwayNoBet
                {
                    LiveIndicator = reader.ToInt("liveindicator"),
                    LiveHomeScore = reader.ToString("livehomescore"),
                    LiveAwayScore = reader.ToString("liveawayscore"),
                    ShowTime = reader.ToString("showtime"),
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid"),
                    IsNeutral = reader.ToInt("isNeutral"),
                    BetCountHomeNoBet = reader.ToInt("betcount_hnb"),
                    BetCountDrawNoBet = reader.ToInt("betcount_dnb"),
                    BetCountAwayNoBet = reader.ToInt("betcount_anb"),
                    HomeOddsHomeNoBet = reader.ToDecimal(homOddsHnbColumn),
                    AwayOddsHomeNoBet = reader.ToDecimal(awayOddsHnbColumn),
                    HomeOddsDrawNoBet = reader.ToDecimal(homeOddsDnbColumn),
                    AwayOddsDrawNoBet = reader.ToDecimal(awayOddsDnbColumn),
                    HomeOddsAwayNoBet = reader.ToDecimal(homeOddsAnbColumn),
                    AwayOddsAwayNoBet = reader.ToDecimal(awayOddsAnbColumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        #endregion Utility
    }
}