// -----------------------------------------------------------------------
// <copyright file="TotalGoalDataAccess.cs" company="Nexcel Solution">
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
    using DataAccess.Interface.TotalBets;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Total goals data access.
    /// </summary>
    public class TotalGoalDataAccess : ITotalGoalDataAccess
    {
        /// <summary>
        /// Get total goal data list.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>List total goal.The result value.</returns>
        public IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel)
        {
            const string UspName = "Lrf_TotalBets_Sel6";
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

            using (var db = new ObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader(uspParams))
                {
                    return SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
                }
            }
        }

        #region Utility

        /// <summary>
        /// Set to list.
        /// </summary>
        /// <param name="reader">The reader value.</param>
        /// <param name="isConvert">The convert value.</param>
        /// <param name="baseCurrency">The base currency value.</param>
        /// <returns>The result value.</returns>
        private static IList<TotalGoal> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<TotalGoal>();

            var total01Column = DataHelper.GetCurrencyColumnName("total01", isConvert, baseCurrency);
            var total23Column = DataHelper.GetCurrencyColumnName("total23", isConvert, baseCurrency);
            var total46Column = DataHelper.GetCurrencyColumnName("total46", isConvert, baseCurrency);
            var total07OverColumn = DataHelper.GetCurrencyColumnName("total7over", isConvert, baseCurrency);
            var totalFirst01Column = DataHelper.GetCurrencyColumnName("fhtotal01", isConvert, baseCurrency);
            var totalFirst23Column = DataHelper.GetCurrencyColumnName("fhtotal23", isConvert, baseCurrency);
            var total04OverColumn = DataHelper.GetCurrencyColumnName("fhtotal4over", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new TotalGoal
                {
                    //// LeagueId = reader.ToInt("leagueid"),
                    MatchId = reader.ToInt("matchid"),
                    HomeId = reader.ToInt("homeID"),
                    AwayId = reader.ToInt("awayID"),
                    IsNeutral = reader.ToInt("isNeutral"),
                    Total01 = reader.ToDecimal(total01Column),
                    Total23 = reader.ToDecimal(total23Column),
                    Total46 = reader.ToDecimal(total46Column),
                    Total7Over = reader.ToDecimal(total07OverColumn),
                    TotalFirstHalf01 = reader.ToDecimal(totalFirst01Column),
                    TotalFirstHalf23 = reader.ToDecimal(totalFirst23Column),
                    Total4Over = reader.ToDecimal(total04OverColumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        #endregion Utility
    }
}