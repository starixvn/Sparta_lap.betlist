namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.Helper;

    public class BetradarVsBetSettingDataAccess : BaseDataAccess, IBetradarVsBetSettingDataAccess
    {
        public IList<BetSetting> GetBetSetting(int custId)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";

            var dataList = new List<BetSetting>();

            using (var db = GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("productid", DbType.Int32, ProductTypes.BetradarVirtualSports);

                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    var sportId = reader.ToString("sporttype");
                    var virtualSportsItem = new BetSetting
                    {
                        MinBet = reader.ToDecimal("minbet"),
                        MaxBet = reader.ToDecimal("maxbet"),
                        MaxPerMatch = reader.ToDecimal("maxpermatch"),
                        SportId = sportId,
                        BetSettingType = CacheRef.GetSportName(sportId)
                    };

                    dataList.Add(virtualSportsItem);
                }
            }

            return dataList;
        }
    }
}