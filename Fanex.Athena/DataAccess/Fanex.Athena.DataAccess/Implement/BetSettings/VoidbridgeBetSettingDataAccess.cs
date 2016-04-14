namespace Fanex.Athena.DataAccess.Implement.BetSettings
{
    using System.Data;
    using Data;
    using Fanex.Athena.DataAccess.Interface.BetSettings;
    using Fanex.Common.Extensions;
    using Models.DataDomain;
    using Models.DTO.Common;

    public class VoidbridgeBetSettingDataAccess : IVoidbridgeBetSettingDataAccess
    {
        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private const int VoidbridgeBettype = 4001;

        private const string UpdateStatusXmlFormat = "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        public bool? GetStatus(int custId)
        {
            var uspName = "Acc_Dep_CustSetting_StatusSettingSelect";
            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddInParameter("ProductId", DbType.Int32, ProductTypes.Voidbridge);
                db.AddInParameter("SettingType", DbType.Int32, ProductStatusSettingType);
                db.AddInParameter("RoleID", DbType.Int32, MemberRoleId);

                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.ToInt("IsDisabled") == 0;
                    }
                }
            }

            return null;
        }

        public IResultInfo UpdateStatus(int userId, int custId, string ip, bool status)
        {
            var uspName = "Acc_Dep_CustSetting_DisableStatUpd";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("IsDisabled", DbType.String, BuildProductStatusXml(status));
                db.AddInParameter("ip", DbType.String, ip);

                db.AddOutParameter("ErrCode", DbType.Int32, 8);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return new ResultInfo
                {
                    Code = (int)db.GetParameterValue("ErrCode"),
                    Message = (string)db.GetParameterValue("ErrMsg")
                };
            }
        }

        private static string BuildProductStatusXml(bool status)
        {
            return string.Format(UpdateStatusXmlFormat, (int)SportType.Voidbridge, VoidbridgeBettype, status ? 0 : 1);
        }
    }
}