// -----------------------------------------------------------------------
// <copyright file="VirtualSportsBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.Helper;

    public class VirtualSportsBetSettingDataAccess : BaseDataAccess, IVirtualSportsBetSettingDataAccess
    {
        public IList<BetSetting> GetBetSetting(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";

            var dataList = new List<BetSetting>();

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("productid", DbType.Int32, productId);

                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    var sportId = reader.ToString("sporttype");
                    var virtualSportsItem = new BetSetting
                    {
                        MinBet = reader.ToDecimal("minbet"),
                        BetValueMin = reader.ToDecimal("minbetmin"),
                        MaxBet = reader.ToDecimal("maxbet"),
                        BetValueMax = reader.ToDecimal("maxbetmax"),
                        MaxPerMatch = reader.ToDecimal("maxpermatch"),
                        MaxPerMatchMax = reader.ToDecimal("maxpermatchmax"),
                        SportId = sportId,
                        BetSettingType = CacheRef.GetSportName(sportId)
                    };

                    dataList.Add(virtualSportsItem);
                }
            }

            return dataList;
        }

        public IResultInfo UpdateBetSetting(int custId, int userId, string ip, string xmlData)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitUpd";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("betlimit", DbType.String, xmlData);
                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 8000);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }
    }
}