// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.MetaModel.Common;
    using VendorAdapter.Interfaces;

    public class LeagueDataAccess : BaseDataAccess, ILeagueDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        private readonly IAppSettings _appSettings;

        public LeagueDataAccess(ICacheRefAdapter cacheRefAdapter, IAppSettings appSettings)
        {
            _cacheRefAdapter = cacheRefAdapter;
            _appSettings = appSettings;
        }

        public IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            using (var db = this.GetObjectDb("Lrf_SelLeagueBySport"))
            {
                db.SetParameterValue("sportId", metaModel.SportId);
                db.SetParameterValue("Fromdate", metaModel.FromDate);
                db.SetParameterValue("Todate", metaModel.ToDate);
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("IsIncludeTestLeague", !_appSettings.IsProduction);
                return ReadLeagues(db);
            }
        }

        public IEnumerable<League> GetRunningLeagues(string agentList, string betTypes)
        {
            const string UspName = "Lrf_LeagueSelRunning";
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("AgentList", agentList);
                db.SetParameterValue("betTypes", betTypes);
                db.SetParameterValue("IsIncludeTestLeague", !_appSettings.IsProduction);
                return ReadLeagues(db);
            }
        }

        private IEnumerable<League> ReadLeagues(IObjectDb db)
        {
            var dataList = new List<League>();
            using (var reader = db.ExecuteReader())
            {
                while (reader.Read())
                {
                    var leagueId = reader.ToInt("leagueid");
                    dataList.Add(new League
                    {
                        LeagueId = leagueId,
                        LeagueName = _cacheRefAdapter.GetLeagueName(leagueId)
                    });
                }

                return dataList;
            }
        }
    }
}