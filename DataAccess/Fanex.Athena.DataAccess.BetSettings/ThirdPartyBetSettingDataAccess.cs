// -----------------------------------------------------------------------
// <copyright file="ThirdPartyBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Data;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Data;
    using Interfaces;

    public class ThirdPartyBetSettingDataAccess : BaseDataAccess, IThirdPartyBetSettingDataAccess
    {
        public ThirdPartyBetSetting GetBetSetting(int custId, int productid)
        {
            var uspName = "Lrf_3rdProductSelCustStatus";
            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddInParameter("ProductId", DbType.Int32, productid);
                db.AddOutParameter("IsEnabled", DbType.Boolean, 1);
                db.AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
                db.AddOutParameter("Currency", DbType.Int32, 1);

                db.ExecuteNonQuery();

                return new ThirdPartyBetSetting()
                {
                    IsEnabled = Converter.ToBool(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled")),
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency"))
                };
            }
        }

        public IResultInfo UpdateStatus(
            int productId,
            int userId,
            int custId,
            string language,
            string ip,
            bool isEnabled)
        {
            var uspName = "Lrf_3rdProductUpdCustStatus";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("productid", DbType.Int32, productId);
                db.AddInParameter("isenabled", DbType.Boolean, isEnabled);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("language", DbType.String, language);

                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

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