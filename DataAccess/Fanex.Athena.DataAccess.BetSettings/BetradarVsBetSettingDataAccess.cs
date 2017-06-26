namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.Core.Helper;

    public class BetradarVsBetSettingDataAccess : BaseDataAccess, IBetradarVsBetSettingDataAccess
    {
        public IList<BetSetting> GetBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelMember_MinMaxBetVS";

            var dataList = new List<BetSetting>();

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);

                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    var sportId = reader.ToString("sporttype");
                    var virtualSportsItem = new BetSetting
                    {
                        MinBet = reader.ToDecimal("minbet"),
                        BetValueMin = reader.ToDecimal("minminbet"),
                        MaxBet = reader.ToDecimal("maxbet"),
                        BetValueMax = reader.ToDecimal("maxmaxbet"),
                        MaxPerMatch = reader.ToDecimal("maxpermatch"),
                        MaxPerMatchMax = reader.ToDecimal("maxmaxpermatch"),
                        MaxPerMatchMin = reader.ToDecimal("maxpermatch_min"),
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