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
    using Session.Interfaces;
    using VendorAdapter.Interfaces;

    public class LeagueDataAccess : BaseDataAccess, ILeagueDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        public LeagueDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage, ICacheRefAdapter cacheRefAdapter)
            : base(appSetting, sessionStorage)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            using (var db = this.GetObjectDb("Lrf_SelLeagueBySport"))
            {
                db.AddInParameter("sportId", DbType.Int32, metaModel.SportId);
                db.AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                return ReadLeagues(db);
            }
        }

        public IEnumerable<League> GetRunningLeagues(string agentList, string betTypes)
        {
            const string UspName = "Lrf_LeagueSelRunning";
            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("AgentList", DbType.String, agentList);
                db.AddInParameter("betTypes", DbType.String, betTypes);
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