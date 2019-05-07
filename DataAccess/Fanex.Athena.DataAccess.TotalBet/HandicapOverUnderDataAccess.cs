// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnderDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core;
    using DataAccess.TotalBet.Interfaces;
    using Extension.Helper;
    using Fanex.Common.Extensions;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class HandicapOverUnderDataAccess : BaseDataAccess, IHandicapOverUnderDataAccess
    {
        public async Task<ReportResultModel<HandicapOverUnder>> GetHandicapOverUnderDataList(HandicapOverUnderMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel1378";
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                PageSize = metaModel.PageSize,
                PageIndex = metaModel.PageIndex,
                AgentList = metaModel.AgentListXml
            };

            using (var db = this.GetObjectDb(SpName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParams).ConfigureAwait(false))
                {
                    var data = SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    reader.NextResult();
                    var totalData = ReadTotalData(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                    return new ReportResultModel<HandicapOverUnder>
                    {
                        DataList = data,
                        TotalData = totalData
                    };
                }
            }
        }

        #region Utility

        private static decimal GetTotalScore(IDataReader reader, string homeColumnName, string awayColumnName)
        {
            var homeScore = reader.ToDecimal(homeColumnName);
            var awayScore = reader.ToDecimal(awayColumnName);

            return homeScore - awayScore;
        }

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

        private static HandicapOverUnder ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (!reader.Read())
            {
                return new HandicapOverUnder();
            }

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

            return new HandicapOverUnder
            {
                FullTimeHandicap = GetRunningStake(reader, hdpHomeScoreColumnName, hdpAwayScoreColumnName),
                FullTimeOverUnder = GetRunningStake(reader, overUnderHomeScoreColumnName, overUnderAwayScoreColumnName),
                FirstTimeHandicap = GetRunningStake(reader, hdpHomeScore1StHalfColumnName, hdpAwayScore1StHalfColumnName),
                FirstTimeOverUnder = GetRunningStake(reader, overUnderHomeScore1StHalfColumnName, overUnderAwayScore1StHalfColumnName)
            };
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