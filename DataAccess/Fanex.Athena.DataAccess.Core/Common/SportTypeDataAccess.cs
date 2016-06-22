// -----------------------------------------------------------------------
// <copyright file="SportTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Resources;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class SportTypeDataAccess : BaseDataAccess, ISportTypeDataAccess
    {
        public SportTypeDataAccess()
            : base()
        {
        }

        public IEnumerable<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, int? productId = null)
        {
            const string UspName = "Lrf_SelSportDetail";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("licenseeId", DbType.Int32, licenseeId);
                    db.AddInParameter("langkey", DbType.String, languageCode);
                    using (var reader = db.ExecuteReader())
                    {
                        var result = new List<SportProductItem>();
                        while (reader.Read())
                        {
                            var parentId = reader["ParentId"] != DBNull.Value ? reader.ToInt("ParentId") : -1;

                            if (!productId.HasValue || productId == parentId)
                            {
                                result.Add(new SportProductItem
                                {
                                    Checked = true,
                                    ID = reader.ToInt("ID"),
                                    Name = reader.ToString("Name"),
                                    ParentId = parentId
                                });
                            }
                        }

                        return result;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get all sports type.
        /// </summary>
        /// <returns>The result value.</returns>
        public IDictionary<int, string> GetAllSports()
        {
            const string UspName = "Lrf_SportsGetAll";
            var result = new SortedDictionary<int, string>();

            using (var db = new ObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sportId = reader.ToInt("Sporttype");
                        var sportName = CacheRef.GetSportName(sportId);

                        result.Add(sportId, sportName);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get sport by date.
        /// </summary>
        /// <returns> The result value.</returns>
        public IList<Sport> GetAllWHSports()
        {
            const string UspName = "Lrf_SportSel";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
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
                    return result;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}