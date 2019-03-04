// -----------------------------------------------------------------------
// <copyright file="BetTypeDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Constant.DataDomain;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using VendorAdapter.Interfaces;

    public class BetTypeDataAccess : BaseDataAccess, IBetTypeDataAccess
    {
        private static readonly IDictionary<int, int> MapBettypeSportBySport = new Dictionary<int, int>
        {
            { 1204, (int)SportType.Soccer },
            { 1235, (int)SportType.Tennis },
            { 8102, (int)SportType.NumberGameHappy5 }
        };

        private static readonly IDictionary<int, int> MapBettypeSportByProduct = new Dictionary<int, int>
        {
            { 1235, (int)SportType.Tennis },
            { 8102, (int)SportType.NumberGameHappy5 },
            { 2803, (int)SportType.VirtualBasketball },
            { 2804, (int)SportType.VirtualBasketball },
            { 2805, (int)SportType.VirtualBasketball },
            { 2806, (int)SportType.VirtualBasketball }
        };

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public BetTypeDataAccess(ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public IList<CheckedItem> GetBettypes(int licenseeId, string xmlSportList, string languageCode)
        {
            const string UspName = "Lrf_SelBetTypeBySport";
            IList<CheckedItem> results = new List<CheckedItem>();

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                db.AddInParameter("sportlist", DbType.String, xmlSportList);
                db.AddInParameter("langkey", DbType.String, languageCode);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var bettypeId = int.Parse(reader["BettypeID"].ToString());
                        var bettypeName = reader["BettypeName"].ToString();
                        if (MapBettypeSportBySport.ContainsKey(bettypeId))
                        {
                            var suffix = _cacheRefAdapter.GetSportName(MapBettypeSportBySport[bettypeId], languageCode);
                            bettypeName += "(" + suffix + ")";
                        }

                        results.Add(new CheckedItem
                        {
                            Checked = true,
                            ID = bettypeId,
                            Name = bettypeName
                        });
                    }
                }
            }

            return results;
        }

        public IList<CheckedItem> GetBettypesByProduct(int licenseeId, string productCode, string languageCode)
        {
            const string UspName = "Lrf_SelBetTypesByProduct";
            IList<CheckedItem> results = new List<CheckedItem>();

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                db.AddInParameter("productcode", DbType.String, productCode);
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = int.Parse(reader["Bettype"].ToString());
                        var name = _cacheRefAdapter.GetBettype(id);
                        if (MapBettypeSportByProduct.ContainsKey(id))
                        {
                            var suffix = _cacheRefAdapter.GetSportName(MapBettypeSportByProduct[id], languageCode);
                            name += "(" + suffix + ")";
                        }

                        results.Add(new CheckedItem
                        {
                            Checked = true,
                            ID = id,
                            Name = name
                        });
                    }
                }
            }

            return results;
        }
    }
}