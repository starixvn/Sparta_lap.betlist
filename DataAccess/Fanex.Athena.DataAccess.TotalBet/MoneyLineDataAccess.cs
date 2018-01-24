// -----------------------------------------------------------------------
// <copyright file="MoneyLineDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using Core;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Helper;
    using Fanex.Common.Extensions;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class MoneyLineDataAccess : BaseDataAccess, IMoneyLineDataAccess
    {
        public ReportResultModel<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel2021";
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
                    var data = SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    reader.NextResult();
                    var totalData = ReadTotalData(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    return new ReportResultModel<MoneyLine>
                    {
                        DataList = data,
                        TotalData = totalData
                    };
                }
            }
        }

        #region Utility

        private static IList<MoneyLine> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
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
                    ForecastAwayFirstHalf = reader.ToDecimal(awayScore1StHalfColumnName),
                    LiveHomeScore = reader.ToString("livehomescore"),
                    LiveAwayScore = reader.ToString("liveawayscore"),
                    LiveIndicator = reader.ToInt("liveindicator"),
                };

                dataList.Add(item);
            }

            return dataList;
        }

        private static MoneyLine ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (reader.Read())
            {
                // Fulltime column name
                var totalHomeScoreColumnName = DataHelper.GetCurrencyColumnName("home_odds", isConvert, baseCurrency);
                var totalAwayScoreColumnName = DataHelper.GetCurrencyColumnName("away_odds", isConvert, baseCurrency);

                // First half column name
                var totalHomeScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("home_odds_HT", isConvert, baseCurrency);
                var totalAwayScore1StHalfColumnName = DataHelper.GetCurrencyColumnName("away_odds_HT", isConvert, baseCurrency);

                return new MoneyLine
                {
                    TotalHomeFt = reader.ToDecimal(totalHomeScoreColumnName),
                    TotalAwayFt = reader.ToDecimal(totalAwayScoreColumnName),
                    TotalHomeFirstHalf = reader.ToDecimal(totalHomeScore1StHalfColumnName),
                    TotalAwayFirstHalf = reader.ToDecimal(totalAwayScore1StHalfColumnName)
                };
            }

            return new MoneyLine();
        }

        #endregion Utility
    }
}