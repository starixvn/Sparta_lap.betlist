namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Xml.Linq;
    using Core;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.Helper;

    public class DcsBetSettingDataAccess : BaseDataAccess, IDcsBetSettingDataAccess
    {
        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private const string UpdateStatusXmlFormat = "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        private static readonly IDictionary<int, int> ProductSporttypeMap = new Dictionary<int, int>()
        {
            { ProductTypes.AllBet, (int)SportType.AllBet },
            { ProductTypes.Voidbridge, (int)SportType.Voidbridge },
            { ProductTypes.FishingWorld, (int)SportType.FishingWorld },
            { ProductTypes.Lottery, (int)SportType.Lottery }
        };

        private static readonly IDictionary<int, int> ProductBettypeMap = new Dictionary<int, int>()
        {
            { ProductTypes.AllBet, 2401 },
            { ProductTypes.Voidbridge, 4001 },
            { ProductTypes.FishingWorld, 1011 },
            { ProductTypes.Lottery, 1021 }
        };

        public bool? GetStatus(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_StatusSettingSelect";
            using (var db = this.GetObjectDb(uspName))
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

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("IsDisabled", DbType.String, BuildProductStatusXml(productId, status));
                db.AddInParameter("ip", DbType.String, ip);

                db.AddOutParameter("ErrCode", DbType.Int32, 8);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                var result = GetResult(db);
                result.Message = GetErrorMessage(result.Message);
                return result;
            }
        }

        public IList<BetSetting> GetBetSetting(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";

            var dataList = new List<BetSetting>();

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("productid", DbType.Int32, productId);

                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    var sportId = reader.ToString("sporttype");
                    var virtualSportsItem = new BetSetting
                    {
                        MinBet = reader.ToDecimal("minbet"),
                        BetValueMin = reader.ToDecimal("minbetmin"),
                        MaxBet = reader.ToDecimal("maxbet"),
                        BetValueMax = reader.ToDecimal("maxbetmax"),
                        MaxPerMatch = reader.ToDecimal("maxpermatch"),
                        MaxPerMatchMax = reader.ToDecimal("maxpermatchmax"),
                        SportId = sportId,
                        BetSettingType = CacheRef.GetSportName(sportId),
                        BetType = reader.ToInt("bettype")
                    };

                    dataList.Add(virtualSportsItem);
                }
            }

            return dataList;
        }

        public IResultInfo UpdateBetSetting(int custId, int userId, string ip, string xmlData)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitUpd";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("betlimit", DbType.String, xmlData);
                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 8000);
                db.ExecuteNonQuery();

                var result = GetResult(db);
                result.Message = GetErrorMessage(result.Message);
                return result;
            }
        }

        private static string BuildProductStatusXml(int productId, bool status)
        {
            return string.Format(UpdateStatusXmlFormat, ProductSporttypeMap[productId], ProductBettypeMap[productId], status ? 0 : 1);
        }

        private static string GetErrorMessage(string xmlErrorMessage)
        {
            if (!string.IsNullOrEmpty(xmlErrorMessage) && xmlErrorMessage.StartsWith("<"))
            {
                try
                {
                    var doc = XDocument.Parse(xmlErrorMessage);
                    return doc.Root.Attribute("Msg").Value;
                }
                catch
                {
                    Logging.LogService.Error("Invalid xml content: " + xmlErrorMessage);
                }
            }

            return xmlErrorMessage;
        }
    }
}