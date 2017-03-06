// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBetDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using Core.Extensions;
    using DataAccess.TotalBet.Interfaces;
    using Fanex.Common.Extensions;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class HomeDrawAwayNoBetDataAccess : BaseDataAccess, IHomeDrawAwayNoBetDataAccess
    {
        public IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel121";
            var uspParams = new
            {
                AgentList = metaModel.AgentListXml,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                TotalRows = metaModel.TotalRows
            };

            using (var db = this.GetObjectDb(SpName))
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