// -----------------------------------------------------------------------
// <copyright file="KenoBetSettingDataAccess.cs" company="">
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
    using Fanex.Common;
    using Interfaces;
    using Model.Core.Helper;
    using VendorAdapter;
    using VendorAdapter.Interfaces;

    public class KenoBetSettingDataAccess : BaseDataAccess, IKenoBetSettingDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        public KenoBetSettingDataAccess()
        {
            _cacheRefAdapter = new CacheRefAdapter();
        }

        public IList<BetSetting> GetBetSettings(int custId)
        {
            var uspName = "Lrf_CustomerSelEditKN";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    return ReadBetSetting(reader);
                }
            }
        }

        public IResultInfo UpdateBetSetting(int custId, int userId, string language, string ip, string xmlData)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBetKN";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("language", DbType.String, language);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("dataxml", DbType.String, xmlData);
                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 8000);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        private IList<BetSetting> ReadBetSetting(IDataReader reader)
        {
            var betSettings = new List<BetSetting>();
            while (reader.Read())
            {
                var gameTypeId = Converter.ToString2(reader["GameTypeID"]);
                var sport = new BetSetting
                {
                    MaxBet = Converter.ToDecimal(reader["MaxBet"]),
                    MinBet = Converter.ToDecimal(reader["MinBet"]),
                    MaxPerMatchMax = Converter.ToDecimal(reader["MaxPerMatch_Max"]),
                    MaxPerMatch = Converter.ToDecimal(reader["MaxPerMatch"]),
                    BetValueMax = Converter.ToDecimal(reader["MaxBet_Min"]),
                    BetValueMin = Converter.ToDecimal(reader["MinBet_Max"]),
                    BetSettingType = _cacheRefAdapter.GetBettype(Converter.ToString2(reader["IBCBetTypeID"])),
                    SportId = gameTypeId
                };

                betSettings.Add(sport);
            }

            return betSettings;
        }
    }
}