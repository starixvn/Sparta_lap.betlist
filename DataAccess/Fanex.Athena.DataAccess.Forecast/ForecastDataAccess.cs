// --------------------------------------------------------------------------
// <copyright file="ForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Extension.Helper;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Forecast.MetaModel;
    using VendorAdapter.Interfaces;

    public class ForecastDataAccess : ForecastBaseDataAccess, IForecastDataAccess
    {
        public ForecastDataAccess(ICacheRefAdapter cacheRefAdapter)
            : base(cacheRefAdapter)
        {
        }

        public async Task<IList<ForecastHandicapOverUnder>> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel)
        {
            const string UspName = "Lrf_Forecast_SelByLeague";

            var paramValues = new
            {
                AgentList = metaModel.AgentListXml,
                LeagueId = metaModel.LeagueId,
                Baseoffsethdp = 0,
                baseoffsetou = 0,
                baseoffseththdp = 0,
                baseoffsethtou = 0
            };
            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(paramValues).ConfigureAwait(false))
                {
                    return GetForecastHandicapOverUnderList(reader, metaModel);
                }
            }
        }

        /// <summary>
        /// Calculate offset.
        /// </summary>
        /// <param name="forecast">Parameter value.</param>
        private static void CalculateOffsetHandicapOverUnder(ForecastHandicapOverUnder forecast)
        {
            var liveHomeScore = forecast.LiveHomeScore;
            var liveAwayScore = forecast.LiveAwayScore;

            var offsetHdp = liveHomeScore - liveAwayScore;
            var offsetOu = liveHomeScore + liveAwayScore;

            forecast.OffsetHdpMinus1 = offsetHdp - 1;
            forecast.OffsetHdp = offsetHdp;
            forecast.OffsetHdpPlus1 = offsetHdp + 1;
#pragma warning disable S109 // Magic numbers should not be used
            forecast.OffsetOuPlus2 = offsetOu + 2;
#pragma warning restore S109 // Magic numbers should not be used
            forecast.OffsetOuPlus1 = offsetOu + 1;
            forecast.OffsetOu = offsetOu;
        }

        /// <summary>
        /// Get forecast handicap over under list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="metaModel">Meta model.</param>
        /// <returns>List result.</returns>
        private IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnderList(IDataReader reader, ForecastHandicapOverUnderMetaModel metaModel)
        {
            var forecasts = new List<ForecastHandicapOverUnder>();

            // Full time column name
            var winLossHdpMinus1 = DataHelper.GetCurrencyColumnName("WinLost_M1_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossHdp = DataHelper.GetCurrencyColumnName("WinLost_0_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossHdpPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOuPlus2 = DataHelper.GetCurrencyColumnName("WinLost_2_OU", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOuPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_OU", metaModel.IsConvert, metaModel.BaseCurrency);
            var winLossOu = DataHelper.GetCurrencyColumnName("WinLost_0_OU", metaModel.IsConvert, metaModel.BaseCurrency);

            // Half time column name
            var halfTimeWinLossHdpMinus1 = DataHelper.GetCurrencyColumnName("WinLost_M1_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossHdp = DataHelper.GetCurrencyColumnName("WinLost_0_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossHdpPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HTHDP", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOuPlus2 = DataHelper.GetCurrencyColumnName("WinLost_2_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOuPlus1 = DataHelper.GetCurrencyColumnName("WinLost_1_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeWinLossOu = DataHelper.GetCurrencyColumnName("WinLost_0_HTOU", metaModel.IsConvert, metaModel.BaseCurrency);

            while (reader.Read())
            {
                var item = new ForecastHandicapOverUnder
                {
                    HandicapMinus1 = reader.ToDecimal(winLossHdpMinus1),
                    Handicap = reader.ToDecimal(winLossHdp),
                    HandicapPlus1 = reader.ToDecimal(winLossHdpPlus1),
                    OverUnderPlus2 = reader.ToDecimal(winLossOuPlus2),
                    OverUnderPlus1 = reader.ToDecimal(winLossOuPlus1),
                    OverUnder = reader.ToDecimal(winLossOu),
                    HtHandicapMinus1 = reader.ToDecimal(halfTimeWinLossHdpMinus1),
                    HtHandicap = reader.ToDecimal(halfTimeWinLossHdp),
                    HtHandicapPlus1 = reader.ToDecimal(halfTimeWinLossHdpPlus1),
                    HtOverUnderPlus2 = reader.ToDecimal(halfTimeWinLossOuPlus2),
                    HtOverUnderPlus1 = reader.ToDecimal(halfTimeWinLossOuPlus1),
                    HtOverUnder = reader.ToDecimal(halfTimeWinLossOu)
                };
                ReadCommonData(reader, item);
                CalculateOffsetHandicapOverUnder(item);
                forecasts.Add(item);
            }

            return forecasts;
        }
    }
}