// -----------------------------------------------------------------------
// <copyright file="SettledMatchCountDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Data;

    /// <summary>
    ///     Settled Match Count.
    /// </summary>
    public class SettledMatchCountDataAccess : BaseDataAccess, ISettledMatchCountDataAccess
    {
        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="isShowAll">Is Show all.</param>
        /// <param name="licenseeId">Licensee ID.</param>
        /// <returns>Result value.</returns>
        public IList<SettledMatchCount> GetDataList(bool isShowAll, int licenseeId)
        {
            var uspParams = new
            {
                isShowAll,
                licenseeid = licenseeId
            };

            using (var db = new ObjectDb("Lrf_Rpt_MatchSelResultEntered"))
            {
                return db.Query<SettledMatchCount>(uspParams).ToList();
            }
        }
    }
}