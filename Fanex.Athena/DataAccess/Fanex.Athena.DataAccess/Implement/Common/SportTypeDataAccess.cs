// -----------------------------------------------------------------------
// <copyright file="SportTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportTypeDataAccess : BaseDataAccess, ISportTypeDataAccess
    {
        public SportTypeDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        /// <summary>
        /// Get products and sports.
        /// </summary>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="languageCode">The language code.</param>
        /// <returns>The data table.</returns>
        public DataTable GetSportsAndProducts(int licenseeId, string languageCode)
        {
            const string UspName = "Lrf_SelSportDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("licenseeId", DbType.Int32, licenseeId);
                    db.AddInParameter("langkey", DbType.String, languageCode);
                    return db.ExecuteDataSet().Tables[0];
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get all sports type.
        /// </summary>
        /// <returns>The result value.</returns>
        public IDictionary<string, int> GetAllSports()
        {
            const string UspName = "Lrf_SportsGetAll";
            var result = new SortedDictionary<string, int>();

            using (var db = new ObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sportId = reader.ToInt("Sporttype");
                        var sportName = CacheRef.GetSportName(sportId);

                        result.Add(sportName, sportId);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get sport by date.
        /// </summary>
        /// <param name="fromDate"> The from date. </param>
        /// <param name="toDate"> The to date. </param>
        /// <returns> The result value.</returns>
        public IList<Sport> SelSportByDate(DateTime fromDate, DateTime toDate)
        {
            const string UspName = "Lrf_DW_SportSelByDate";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("fromdate", System.Data.DbType.DateTime, fromDate);
                    db.AddInParameter("todate", System.Data.DbType.DateTime, toDate);
                    db.AddInParameter("@balanceupday", System.Data.DbType.DateTime, BalanceUp.BalanceUpDay);
                    var result = new List<Sport>();
                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var sportId = reader.ToInt("Sporttype");
                            var sportName = CacheRef.GetSportName(sportId);

                            result.Add(new Sport(sportId.ToString(), sportName));
                        }
                    }

                    result.Insert(0, new Sport("0", ViewResources.All));
                    result.Add(new Sport("-1", ViewResources.OtherSport));
                    return result;
                }
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}