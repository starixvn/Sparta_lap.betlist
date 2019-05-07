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
    using Core.Extensions;
    using Extension.Helper;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.MetaModel;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;
    using VendorAdapter.Interfaces;

    public class ForecastOneXTwoDataAccess : ForecastBaseDataAccess, IForecastOneXTwoDataAccess
    {
        public ForecastOneXTwoDataAccess(ICacheRefAdapter cacheRefAdapter)
            : base(cacheRefAdapter)
        {
        }

        public async Task<IList<ForecastOneXTwo>> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            const string UspName = "Lrf_Forecast1x2_SelByLeague";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("LeagueId", metaModel.LeagueId);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return GetForecastOneXTwoList(metaModel, reader);
                }
            }
        }

        public async Task<IList<ForecastOneXTwo>> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel)
        {
            const string UspName = "Lrf_Forecast_Sel1X2";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("matchid", metaModel.MatchId);
                db.SetParameterValue("betType", metaModel.BetType);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return GetForecastOneXTwoList(metaModel, reader);
                }
            }
        }

        private IList<ForecastOneXTwo> GetForecastOneXTwoList(BaseMetaModel metaModel, IDataReader reader)
        {
            var forecasts = new List<ForecastOneXTwo>();
            const string ExtraColumnName = "isExtra";
            var homeWinLossColumn = DataHelper.GetCurrencyColumnName("hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var drawWinLossColumn = DataHelper.GetCurrencyColumnName("dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var awayWinLossColumn = DataHelper.GetCurrencyColumnName("al", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeHomeWinLossColumn = DataHelper.GetCurrencyColumnName("ht_hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeDrawWinLossColumn = DataHelper.GetCurrencyColumnName("ht_dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var halfTimeAwayWinLossColumn = DataHelper.GetCurrencyColumnName("ht_al", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraHomeWinLossColumn = DataHelper.GetCurrencyColumnName("x_hl", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraDrawWinLossColumn = DataHelper.GetCurrencyColumnName("x_dl", metaModel.IsConvert, metaModel.BaseCurrency);
            var extraAwayWinLossColumn = DataHelper.GetCurrencyColumnName("x_al", metaModel.IsConvert, metaModel.BaseCurrency);
            var hasExtraColumn = reader.HasColumn(ExtraColumnName);
            while (reader.Read())
            {
                var item = new ForecastOneXTwo();
                ReadCommonData(reader, item);
                if (!hasExtraColumn || reader.ToInt(ExtraColumnName) == 0)
                {
                    item.IsExtra = false;
                    item.HomeWinLoss = reader.ToDecimal(homeWinLossColumn);
                    item.DrawWinLoss = reader.ToDecimal(drawWinLossColumn);
                    item.AwayWinLoss = reader.ToDecimal(awayWinLossColumn);
                    item.HalfTimeHomeWinLoss = reader.ToDecimal(halfTimeHomeWinLossColumn);
                    item.HalfTImeDrawWinLoss = reader.ToDecimal(halfTimeDrawWinLossColumn);
                    item.HalfTimeAwayWinLoss = reader.ToDecimal(halfTimeAwayWinLossColumn);
                }
                else
                {
                    item.IsExtra = true;
                    item.HalfTimeHomeWinLoss = reader.ToDecimal(extraHomeWinLossColumn);
                    item.HalfTImeDrawWinLoss = reader.ToDecimal(extraDrawWinLossColumn);
                    item.HalfTimeAwayWinLoss = reader.ToDecimal(extraAwayWinLossColumn);
                }

                forecasts.Add(item);
            }

            return forecasts;
        }
    }
}