// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class LeagueDataAccess : BaseDataAccess, ILeagueDataAccess
    {
        /// <summary>
        /// Get leagues by sport Id.
        /// </summary>
        /// <param name="metaModel">Meta model object.</param>
        /// <returns>List of league.</returns>
        public IList<League> GetLeaguesBySport(LeagueMetaModel metaModel)
        {
            var uspName = "Lrf_SelLeagueBySport";
            var leagues = new List<League>();

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                db.AddInParameter("CurrencyID", DbType.Int32, metaModel.CurrencyId);
                db.AddInParameter("userID", DbType.Int32, metaModel.UserId);
                db.AddInParameter("sportId", DbType.Int32, metaModel.SportId);
                db.AddInParameter("Fromdate", DbType.DateTime, metaModel.FromDate);
                db.AddInParameter("Todate", DbType.DateTime, metaModel.ToDate);

                using (var reader = db.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var league = new League
                            {
                                LeagueId = reader.ToInt("leagueid")                                
                            };
                            leagues.Add(league);
                        }
                    }
                }
            }

            return leagues;
        }
    }
}