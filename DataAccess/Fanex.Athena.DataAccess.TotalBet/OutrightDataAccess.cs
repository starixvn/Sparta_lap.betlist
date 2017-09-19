// -----------------------------------------------------------------------
// <copyright file="OutrightDataAccess.cs" company="">
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
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class OutrightDataAccess : BaseDataAccess, IOutrightDataAccess
    {
        public IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel)
        {
            const string SpName = "Lrf_TotalBets_Sel10";
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