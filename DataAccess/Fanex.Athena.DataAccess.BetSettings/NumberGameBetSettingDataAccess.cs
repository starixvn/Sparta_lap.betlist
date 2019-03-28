// -----------------------------------------------------------------------
// <copyright file="NumberGameBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Utility;

    public class NumberGameBetSettingDataAccess : BaseDataAccess, INumberGameBetSettingDataAccess
    {
        private readonly IBetSettingReader _betSettingReader;

        public NumberGameBetSettingDataAccess(IBetSettingReader betSettingReader)
        {
            _betSettingReader = betSettingReader;
        }

        public async Task<IList<BetSetting>> GetBetSettingAsync(int custId)
        {
            var uspName = "Lrf_CustomerSelEditNG";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("CustID", custId);
                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (reader.Read())
                    {
                        return RenderNumberGameBetSetting(reader);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(NumberGameBetSetting param)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet161";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", param.CustId);
                db.SetParameterValue("userid", param.UserId);
                db.SetParameterValue("minbetv161", param.MinBet);
                db.SetParameterValue("maxbetv161", param.MaxBet);
                db.SetParameterValue("maxpermatchv161", param.MaxPerMatch);
                db.SetParameterValue("maxperballv161", param.MaxPerBall);
                db.SetParameterValue("language", param.Language);
                db.SetParameterValue("ip", param.Ip);
                AddResultParam(db);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return GetResult(db);
            }
        }

        private IList<BetSetting> RenderNumberGameBetSetting(IDataReader reader)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var numberGameList = new List<BetSetting>();

            numberGameList.Add(_betSettingReader.ReadForSport(reader, ((int)SportType.NumberGame).ToString(), true));

            return numberGameList;
        }
    }
}