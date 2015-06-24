// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalDataAccess.cs" company="Nexcel Solution">
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
    /// First goal/last goal data access.
    /// </summary>
    public class FirstGoalLastGoalDataAccess : IFirstGoalLastGoalDataAccess
    {
        /// <summary>
        /// Get FG/LG data list.
        /// </summary>
        /// <param name="metaModel">FG/LG MetaModel value.</param>
        /// <returns>A list of FG/LG.</returns>
        public IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel, out FirstGoalLastGoal totalData)
        {
            const string SpName = "Lrf_TotalBets_Sel14";
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

            IList<FirstGoalLastGoal> data;
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
        /// Set data to list.
        /// </summary>
        /// <param name="reader">Data reader value.</param>
        /// <param name="isConvert">Is Convert value.</param>
        /// <param name="baseCurrency">Base Currency value.</param>
        /// <returns>A list of FG/LG.</returns>
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
                var homeFirstHalfFirstGoalColumn = DataHelper.GetCurrencyColumnName("winlost_1HhomeFG", isConvert, baseCurrency);

                return new FirstGoalLastGoal
                {                    
                    HomeFirstGoal = reader.ToDecimal(homeFirstGoalColumn),                    
                    HomeFirstHalfFirstGoal = reader.ToNullableDecimal(homeFirstHalfFirstGoalColumn),                    
                };
            }

            return new FirstGoalLastGoal();
        }

        #endregion Utility
    }
}