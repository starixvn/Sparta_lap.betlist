// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalDataAccess.cs" company="">
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
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Common.Extensions;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using Session.Interfaces;

    public class FirstGoalLastGoalDataAccess : BaseDataAccess, IFirstGoalLastGoalDataAccess
    {
        public FirstGoalLastGoalDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ReportResultModel<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel14";

            using (var db = GetObjectDb(SpName))
            {
                var uspParams = new
                {
                    AgentList = metaModel.AgentListXml,
                    PageSize = metaModel.PageSize,
                    PageIndex = metaModel.PageIndex,
                    TotalRows = metaModel.TotalRows
                };

                using (var reader = db.ExecuteReader(uspParams))
                {
                    var data = SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);

                    reader.NextResult();
                    var totalData = ReadTotalData(reader, metaModel.IsConvert, metaModel.BaseCurrency);

                    return new ReportResultModel<FirstGoalLastGoal>
                    {
                        DataList = data,
                        TotalData = totalData
                    };
                }
            }
        }

        #region Utility

        private static IList<FirstGoalLastGoal> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<FirstGoalLastGoal>();

            var homeFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_homeFG", isConvert, baseCurrency);
            var homeLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_homeLG", isConvert, baseCurrency);
            var awayFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_awayFG", isConvert, baseCurrency);
            var awayLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_awayLG", isConvert, baseCurrency);
            var nonGoalColumn = DataHelper.GetCurrencyColumnName("winlost_NoGoal", isConvert, baseCurrency);
            var homeFirstHalfFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HhomeFG", isConvert, baseCurrency);
            var homeFirstHalfLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HhomeLG", isConvert, baseCurrency);
            var awayFirstHalfFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HawayFG", isConvert, baseCurrency);
            var awayFirstHalfLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HawayLG", isConvert, baseCurrency);
            var firstHalfNonGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HNoGoal", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new FirstGoalLastGoal
                {
                    LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    ShowTime = reader.ToString("showtime"),
                    IsNeutral = reader.ToInt("isNeutral"),
                    HomeId = reader.ToInt("homeid"),
                    AwayId = reader.ToInt("awayid"),
                    HomeFirstGoal = reader.ToDecimal(homeFirstGoalColumn),
                    HomeLastGoal = reader.ToDecimal(homeLastGoalColumn),
                    AwayFirstGoal = reader.ToDecimal(awayFirstGoalColumn),
                    AwayLastGoal = reader.ToDecimal(awayLastGoalColumn),
                    NoGoal = reader.ToDecimal(nonGoalColumn),
                    HomeFirstHalfFirstGoal = reader.ToNullableDecimal(homeFirstHalfFirstGoalColumn),
                    HomeFirstHalfLastGoal = reader.ToNullableDecimal(homeFirstHalfLastGoalColumn),
                    AwayFirstHalfFirstGoal = reader.ToNullableDecimal(awayFirstHalfFirstGoalColumn),
                    AwayFirstHalfLastGoal = reader.ToNullableDecimal(awayFirstHalfLastGoalColumn),
                    FirstHalfNoGoal = reader.ToNullableDecimal(firstHalfNonGoalColumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        private static FirstGoalLastGoal ReadTotalData(IDataReader reader, bool isConvert, string baseCurrency)
        {
            if (reader.Read())
            {
                var homeFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_homeFG", isConvert, baseCurrency);
                var homeLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_homeLG", isConvert, baseCurrency);
                var awayFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_awayFG", isConvert, baseCurrency);
                var awayLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_awayLG", isConvert, baseCurrency);
                var nonGoalColumn = DataHelper.GetCurrencyColumnName("winlost_NoGoal", isConvert, baseCurrency);
                var homeFirstHalfFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HhomeFG", isConvert, baseCurrency);
                var homeFirstHalfLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HhomeLG", isConvert, baseCurrency);
                var awayFirstHalfFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HawayFG", isConvert, baseCurrency);
                var awayFirstHalfLastGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HawayLG", isConvert, baseCurrency);
                var firstHalfNonGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HNoGoal", isConvert, baseCurrency);

                return new FirstGoalLastGoal
                {
                    HomeFirstGoal = SumColumns(reader, homeFirstGoalColumn, homeLastGoalColumn, awayFirstGoalColumn, awayLastGoalColumn, nonGoalColumn),
                    HomeFirstHalfFirstGoal = SumColumns(
                                                reader, homeFirstHalfFirstGoalColumn, homeFirstHalfLastGoalColumn, awayFirstHalfFirstGoalColumn, awayFirstHalfLastGoalColumn, firstHalfNonGoalColumn)
                };
            }

            return new FirstGoalLastGoal();
        }

        private static decimal SumColumns(IDataReader reader, params string[] columns)
        {
            decimal result = 0;
            foreach (var column in columns)
            {
                result += reader.ToDecimal(column);
            }

            return result;
        }

        #endregion Utility
    }
}