// -----------------------------------------------------------------------
// <copyright file="AgCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Data;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Data;
    using Interfaces;
    using Models.DTO.BetSettings;
    using Models.DTO.BetSettings.AgCasino;

    public class AgCasinoBetSettingDataAccess : BaseDataAccess, IAgCasinoBetSettingDataAccess
    {
        public AgCasinoBetSetting GetBetSetting(int custId)
        {
            var uspName = "Lrf_AGC_SelCustSetting";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddOutParameter("Currency", DbType.Int32, int.MaxValue);
                db.AddOutParameter("AGGroup", DbType.String, 20);
                db.AddOutParameter("IsEnabled", DbType.Boolean, 1);
                db.AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
                db.AddOutParameter("CheckedGroups", DbType.String, 500);

                db.ExecuteNonQuery();

                return new AgCasinoBetSetting()
                {
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency")),
                    AgGroup = Converter.ToString2(db.GetParameterValue("AGGroup")),
                    IsEnabled = Converter.ToBool(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled")),
                    CheckedGroups = Converter.ToString2(db.GetParameterValue("CheckedGroups")).Split(',')
                };
            }
        }

        public IResultInfo UpdateBetSetting(CommonBetSetting data, string group)
        {
            var uspName = "Lrf_AGC_UpdCustSetting";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("aggroup", DbType.String, group);
                db.AddInParameter("ip", DbType.String, data.Ip);
                db.AddInParameter("language", DbType.String, data.Language);
                db.AddInParameter("userid", DbType.Int32, data.UserId);
                db.AddInParameter("custid", DbType.Int32, data.CustId);

                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateStatus(int userId, int custId, string language, string ip, bool isEnabled)
        {
            var uspName = "Lrf_AGC_UpdCustStatus";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("isenabled", DbType.Boolean, isEnabled);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("language", DbType.String, language);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("custid", DbType.Int32, custId);

                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }
    }
}