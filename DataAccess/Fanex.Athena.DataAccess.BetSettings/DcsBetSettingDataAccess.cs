namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Common.Extensions;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;

    public class DcsBetSettingDataAccess : IDcsBetSettingDataAccess
    {
        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private const string UpdateStatusXmlFormat = "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        private static readonly IDictionary<int, int> ProductSporttypeMap = new Dictionary<int, int>()
        {
            { ProductTypes.AllBet, (int)SportType.AllBet },
            { ProductTypes.Voidbridge, (int)SportType.Voidbridge },
            { ProductTypes.FishingWorld, (int)SportType.FishingWorld }
        };

        private static readonly IDictionary<int, int> ProductBettypeMap = new Dictionary<int, int>()
        {
            { ProductTypes.AllBet, 2401 },
            { ProductTypes.Voidbridge, 4001 },
            { ProductTypes.FishingWorld, 1011 }
        };

        public bool? GetStatus(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_StatusSettingSelect";
            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddInParameter("ProductId", DbType.Int32, productId);
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

        public IResultInfo UpdateStatus(int userId, int custId, string ip, bool status, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_DisableStatUpd";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("IsDisabled", DbType.String, BuildProductStatusXml(productId, status));
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

        private static string BuildProductStatusXml(int productId, bool status)
        {
            return string.Format(UpdateStatusXmlFormat, ProductSporttypeMap[productId], ProductBettypeMap[productId], status ? 0 : 1);
        }
    }
}