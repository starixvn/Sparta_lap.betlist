// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Threading.Tasks;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO.AgCasino;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.BetSettings.DTO;

    public class AgCasinoBetSettingDataAccess : BaseDataAccess, IAgCasinoBetSettingDataAccess
    {
        public async Task<AgCasinoBetSetting> GetBetSettingAsync(int custId)
        {
            const string UspName = "Lrf_AGC_SelCustSetting";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("Currency", null);
                db.SetParameterValue("AGGroup", null);
                db.SetParameterValue("IsEnabled", null);
                db.SetParameterValue("IsLineEnabled", null);
                db.SetParameterValue("CheckedGroups", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return new AgCasinoBetSetting
                {
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency")),
                    AgGroup = Converter.ToString2(db.GetParameterValue("AGGroup")),
                    IsEnabled = Converter.ToBool(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled")),
                    CheckedGroups = Converter.ToString2(db.GetParameterValue("CheckedGroups")).Split(',')
                };
            }
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting data, string group)
        {
            const string UspName = "Lrf_AGC_UpdCustSetting";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("aggroup", group);
                db.SetParameterValue("ip", data.Ip);
                db.SetParameterValue("language", data.Language);
                db.SetParameterValue("userid", data.UserId);
                db.SetParameterValue("custid", data.CustId);

                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return GetResult(db);
            }
        }

        public async Task<IResultInfo> UpdateStatusAsync(int userId, int custId, string language, string ip, bool isEnabled)
        {
            const string UspName = "Lrf_AGC_UpdCustStatus";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("isenabled", isEnabled);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("language", language);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("custid", custId);

                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return GetResult(db);
            }
        }
    }
}