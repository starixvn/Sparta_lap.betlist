// -----------------------------------------------------------------------
// <copyright file="OutrightDataAccess.cs" company="Nexcel Solution">
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
    using Core.Extensions;
    using Data;
    using DataAccess.Interface.TotalBets;
    using Fanex.Common.Extensions;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Outright data access.
    /// </summary>
    public class OutrightDataAccess : IOutrightDataAccess
    {
        /// <summary>
        /// Get outright data list.
        /// </summary>
        /// <param name="metaModel">Outright MetaModel value.</param>
        /// <returns>A list of Outright.</returns>
        public IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel10";
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
                    return SetToList(reader, metaModel.IsConvert, metaModel.BaseCurrency);
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
        /// <returns>A list of Handicap Over/Under.</returns>
        private static IList<Outright> SetToList(IDataReader reader, bool isConvert, string baseCurrency)
        {
            var dataList = new List<Outright>();

            var forecastColumn = DataHelper.GetCurrencyColumnName("totalwinlost", isConvert, baseCurrency);
            var balanceColumn = DataHelper.GetCurrencyColumnName("balance", isConvert, baseCurrency);

            while (reader.Read())
            {
                var item = new Outright
                {
                    LeagueId = reader.ToInt("leagueid"),
                    TeamId = reader.ToInt("teamid"),
                    MatchId = reader.ToInt("orid"),
                    Forecast = reader.ToDecimal(forecastColumn),
                    Balance = reader.ToDecimal(balanceColumn)
                };

                dataList.Add(item);
            }

            return dataList;
        }

        #endregion Utility
    }
}