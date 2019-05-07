// -----------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Threading.Tasks;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;

    public class ThirdPartyBetSettingDataAccess : BaseDataAccess, IThirdPartyBetSettingDataAccess
    {
        public async Task<ThirdPartyBetSetting> GetBetSettingAsync(int custId, int productid)
        {
            const string UspName = "Lrf_3rdProductSelCustStatus";
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ProductId", productid);
                db.SetParameterValue("IsEnabled", null);
                db.SetParameterValue("IsLineEnabled", null);
                db.SetParameterValue("Currency", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                return new ThirdPartyBetSetting
                {
                    IsEnabled = Converter.ToBool(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled")),
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency"))
                };
            }
        }

        public async Task<IResultInfo> UpdateStatusAsync(
            int productId,
            int userId,
            int custId,
            string language,
            string ip,
            bool isEnabled)
        {
            const string UspName = "Lrf_3rdProductUpdCustStatus";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("productid", productId);
                db.SetParameterValue("isenabled", isEnabled);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("language", language);

                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var errorMessage = db.GetParameterValue("ErrMsg");

                return new ResultInfo
                {
                    Code = 0,
                    Message = Converter.ToString2(errorMessage)
                };
            }
        }
    }
}