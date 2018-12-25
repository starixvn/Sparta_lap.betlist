// -----------------------------------------------------------------------
// <copyright file="SportTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Constant.DataDomain;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Session.Interfaces;
    using VendorAdapter.Interfaces;

    public class SportTypeDataAccess : BaseDataAccess, ISportTypeDataAccess
    {
        private static readonly int[] HiddenSports = new[]
        {
            (int)SportType.UnknowThaiSport,
            (int)SportType.Cricket // Duplicated with sport 50
        };

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public SportTypeDataAccess(
            ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IEnumerable<SportProductItem> GetSportsAndProducts(int licenseeId, string languageCode, string productCodes = "")
        {
            const string UspName = "Lrf_SelSportDetail";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("licenseeId", DbType.Int32, licenseeId);
                db.AddInParameter("langkey", DbType.String, languageCode);
                db.AddInParameter("ProductCodes", DbType.String, productCodes);
                using (var reader = db.ExecuteReader())
                {
                    var result = new List<SportProductItem>();
                    while (reader.Read())
                    {
                        var id = reader.ToInt("ID");
                        if (!HiddenSports.Contains(id))
                        {
                            result.Add(new SportProductItem
                            {
                                Checked = true,
                                ID = id,
                                Name = reader.ToString("Name"),
                                ParentId = reader["ParentId"] != DBNull.Value ? reader.ToInt("ParentId") : -1
                            });
                        }
                    }

                    return result;
                }
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

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var sportId = reader.ToInt("Sporttype");
                        if (!HiddenSports.Contains(sportId))
                        {
                            var sportName = _cacheRefAdapter.GetSportName(sportId);
                            result.Add(sportId, sportName);
                        }
                    }
                }
            }

            return result;
        }
    }
}