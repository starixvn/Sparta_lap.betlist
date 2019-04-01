// -----------------------------------------------------------------------
// <copyright file="KenoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Interfaces;
    using VendorAdapter.Interfaces;

    public class KenoBetSettingDataAccess : BaseDataAccess, IKenoBetSettingDataAccess
    {
        private readonly ICacheRefAdapter _cacheRefAdapter;

        public KenoBetSettingDataAccess(
            ICacheRefAdapter cacheRefAdapter)
        {
            _cacheRefAdapter = cacheRefAdapter;
        }

        public async Task<IList<BetSetting>> GetBetSettingsAsync(int custId)
        {
            var uspName = "Lrf_CustomerSelEditKN";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    return ReadBetSetting(reader);
                }
            }
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(int custId, int userId, string language, string ip, string xmlData)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBetKN";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("language", language);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("dataxml", xmlData);
                db.SetParameterValue("errcode", null);
                db.SetParameterValue("errmsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

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