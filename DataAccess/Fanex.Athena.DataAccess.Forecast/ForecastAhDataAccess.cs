// --------------------------------------------------------------------------
// <copyright file="ForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast
{
    using System.Data;
    using Interfaces;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;
    using VendorAdapter.Interfaces;

    public class ForecastAhDataAccess : ForecastBaseDataAccess, IForecastAhDataAccess
    {
        public ForecastAhDataAccess(ICacheRefAdapter cacheRefAdapter)
            : base(cacheRefAdapter)
        {
        }

        public ForecastAH GetForecastTopOddsByMatch(int matchId, int betType)
        {
            const string UspName = "Lrf_Forecast_OddsSelTopByMatch";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("matchid", matchId);
                db.SetParameterValue("betType", betType);
                var item = db.QueryEntity<ForecastAH>();
                AssignTeamNames(item);
                return item;
            }
        }

        public DataTable GetForecastHandicap(ForecastAHMetaModel metaModel, int userId)
        {
            const string UspName = "Lrf_Forecast_SelHDP";
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("matchid", metaModel.MatchId);
                db.SetParameterValue("betType", metaModel.BetType);
                var dataset = db.ExecuteDataSet();
                return dataset != null && dataset.Tables.Count > 0 ? dataset.Tables[0] : null;
            }
        }

        public DataTable GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId)
        {
            const string UspName = "Lrf_Forecast_SelOU";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("matchid", metaModel.MatchId);
                db.SetParameterValue("betType", metaModel.BetType);
                var dataset = db.ExecuteDataSet();
                return dataset != null && dataset.Tables.Count > 0 ? dataset.Tables[0] : null;
            }
        }
    }
}