namespace Fanex.Athena.DataAccess.BetSettings
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Constant.DataDomain;
    using Core;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.Core.DTO.Common;
    using Service.Core.Utility.Interfaces;
    using VendorAdapter.Interfaces;

    public class DcsBetSettingDataAccess : BaseDataAccess, IDcsBetSettingDataAccess
    {
        private const int MemberRoleId = 1;

        private const int ProductStatusSettingType = 4;

        private const string UpdateStatusXmlFormat = "<ROOT><ProductStatus SportType=\"{0}\" BetType=\"{1}\" IsDisabled=\"{2}\" IsDisabledDownline=\"0\" /></ROOT>";

        private static readonly IDictionary<int, int> ProductSporttypeMap = new Dictionary<int, int>
        {
            { ProductTypes.AllBet, (int)SportType.AllBet },
            { ProductTypes.Voidbridge, (int)SportType.Voidbridge },
            { ProductTypes.FishingWorld, (int)SportType.FishingWorld },
            { ProductTypes.Lottery, (int)SportType.Lottery },
            { ProductTypes.Exchange, (int)SportType.Exchange },
            { ProductTypes.PP, (int)SportType.PP },
            { ProductTypes.Sportsbooks2, (int)SportType.Esports},
        };

        private static readonly IDictionary<int, int> ProductBettypeMap = new Dictionary<int, int>
        {
            { ProductTypes.AllBet, 2401 },
            { ProductTypes.Voidbridge, 4001 },
            { ProductTypes.FishingWorld, 1011 },
            { ProductTypes.Lottery, 1021 },
            { ProductTypes.Exchange, 8001 },
            { ProductTypes.PP, 4101 },
            { ProductTypes.Sportsbooks2, 1}
        };

        private readonly ICoreLogger _logger;

        private readonly ICacheRefAdapter _cacheRefAdapter;

        public DcsBetSettingDataAccess(
            ICoreLogger logger,
            ICacheRefAdapter cacheRefAdapter)
        {
            _logger = logger;
            _cacheRefAdapter = cacheRefAdapter;
        }

        public async Task<bool?> GetStatusAsync(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_StatusSettingSelect";
            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("CustID", custId);
                db.SetParameterValue("ProductId", productId);
                db.SetParameterValue("SettingType", ProductStatusSettingType);
                db.SetParameterValue("RoleID", MemberRoleId);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (reader.Read())
                    {
                        return reader.ToInt("IsDisabled") == 0;
                    }
                }
            }

            return null;
        }

        public async Task<IResultInfo> UpdateStatusAsync(int userId, int custId, string ip, bool status, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_DisableStatUpd";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("IsDisabled", BuildProductStatusXml(productId, status));
                db.SetParameterValue("ip", ip);

                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var result = GetResult(db);
                result.Message = GetErrorMessage(result.Message);
                return result;
            }
        }

        public async Task<IList<BetSetting>> GetBetSettingAsync(int custId, int productId)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitSettingSelect";

            var dataList = new List<BetSetting>();

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("productid", productId);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
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
                            BetSettingType = _cacheRefAdapter.GetSportName(sportId),
                            BetType = reader.ToInt("bettype")
                        };

                        dataList.Add(virtualSportsItem);
                    }
                }
            }

            return dataList;
        }

        public async Task<IResultInfo> UpdateBetSettingAsync(int custId, int userId, string ip, string xmlData)
        {
            var uspName = "Acc_Dep_CustSetting_BetLimitUpd";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("userid", userId);
                db.SetParameterValue("ip", ip);
                db.SetParameterValue("betlimit", xmlData);
                db.SetParameterValue("errcode", null);
                db.SetParameterValue("errmsg", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var result = GetResult(db);
                result.Message = GetErrorMessage(result.Message);
                return result;
            }
        }

        private static string BuildProductStatusXml(int productId, bool status)
        {
            return string.Format(UpdateStatusXmlFormat, ProductSporttypeMap[productId], ProductBettypeMap[productId], status ? 0 : 1);
        }

        private string GetErrorMessage(string xmlErrorMessage)
        {
            if (!string.IsNullOrEmpty(xmlErrorMessage) && xmlErrorMessage.StartsWith("<"))
            {
                try
                {
                    var doc = XDocument.Parse(xmlErrorMessage);
                    return doc.Root.Attribute("Msg").Value;
                }
                catch (Exception)
                {
                    _logger.Error("Invalid xml content: " + xmlErrorMessage);
                }
            }

            return xmlErrorMessage;
        }
    }
}