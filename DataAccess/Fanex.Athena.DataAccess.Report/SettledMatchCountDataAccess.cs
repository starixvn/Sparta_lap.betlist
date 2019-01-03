// -----------------------------------------------------------------------
// <copyright file="SettledMatchCountDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;

    public class SettledMatchCountDataAccess : BaseDataAccess, ISettledMatchCountDataAccess
    {
        public IList<SettledMatchCount> GetDataList(bool isShowAll, int licenseeId)
        {
            var uspParams = new
            {
                isShowAll,
                licenseeid = licenseeId
            };

            using (var db = this.GetObjectDb("Lrf_Rpt_MatchSelResultEntered"))
            {
                return db.Query<SettledMatchCount>(uspParams).ToList();
            }
        }
    }
}