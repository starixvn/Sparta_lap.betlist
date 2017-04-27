// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.Core.MetaModel.Common;

    public class LeagueDataAccess : BaseDataAccess, ILeagueDataAccess
    {
        public IEnumerable<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            using (var db = this.GetObjectDb("Lrf_SelLeagueBySport"))
            {
                db.AddInParameter("sportId", DbType.Int32, metaModel.SportId);
                db.AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);
                db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                return db.Query<League>();
            }
        }
    }
}