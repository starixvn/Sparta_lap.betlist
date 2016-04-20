// -----------------------------------------------------------------------
// <copyright file="BetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using Common;
    using Core;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.BetSettings.Interfaces;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    public class BetSettingDataAccess : BaseDataAccess, IBetSettingDataAccess
    {
        public IList<BetSetting> GetSportBookBetSetting(int custId, ref EsportSettingType esportSetting)
        {
            var uspName = "Lrf_CustomerSelEditCustomer";
            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        esportSetting = reader.HasColumn("DisableEsports") ?
                            (EsportSettingType)Converter.ToInt(reader["DisableEsports"])
                            : EsportSettingType.NoneOffering;
                        return RenderSportBookBetSetting(reader);
                    }
                }
            }

            return new List<BetSetting>();
        }

        public IList<BetSetting> GetVirtualSportsBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelMember_MinMaxBetVS";

            var dataList = new List<BetSetting>();

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);

                var reader = db.ExecuteReader();
                while (reader.Read())
                {
                    var sportId = reader.ToString("sporttype");
                    var virtualSportsItem = new BetSetting
                    {
                        MinBet = reader.ToDecimal("minbet"),
                        BetValueMin = reader.ToDecimal("minminbet"),
                        MaxBet = reader.ToDecimal("maxbet"),
                        BetValueMax = reader.ToDecimal("maxmaxbet"),
                        MaxPerMatch = reader.ToDecimal("maxpermatch"),
                        MaxPerMatchMax = reader.ToDecimal("maxmaxpermatch"),
                        MaxPerMatchMin = reader.ToDecimal("maxpermatch_min"),
                        SportId = sportId
                    };

                    virtualSportsItem.BetSettingType = CacheRef.GetSportName(sportId);

                    dataList.Add(virtualSportsItem);
                }
            }

            return dataList;
        }

        public IResultInfo UpdateSportsBookBetSetting(CommonBetSetting data, IDictionary<string, object> setting)
        {
            var uspName = "Lrf_CustomerUpdMember_All";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, data.LicenseeId);
                db.AddInParameter("ip", DbType.String, data.Ip);
                db.AddInParameter("language", DbType.String, data.Language);
                db.AddInParameter("userid", DbType.Int32, data.UserId);
                db.AddInParameter("custid", DbType.Int32, data.CustId);
                foreach (var item in setting)
                {
                    if (item.Key.Contains("UpdateMinMaxBet"))
                    {
                        db.AddInParameter(item.Key, DbType.Boolean, item.Value);
                    }
                    else
                    {
                        db.AddInParameter(item.Key, DbType.Decimal, item.Value);
                    }
                }

                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled)
        {
            var uspName = "Lrf_CustomerUpdEsportStatus";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("ip", DbType.String, setting.Ip);
                db.AddInParameter("Langkey", DbType.String, setting.Language);
                db.AddInParameter("userid", DbType.Int32, setting.UserId);
                db.AddInParameter("custid", DbType.Int32, setting.CustId);
                db.AddInParameter("DisableEsports", DbType.Boolean, isDisabled);
                db.AddOutParameter("ErrCode", DbType.String, 600);
                db.AddOutParameter("ErrMsg", DbType.String, 8000);

                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        public IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, string xmlData)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBetVS";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("language", DbType.String, language);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("dataxml", DbType.String, xmlData);
                db.AddOutParameter("errcode", DbType.Int32, 4);
                db.AddOutParameter("errmsg", DbType.String, 8000);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        private IList<BetSetting> RenderSportBookBetSetting(IDataReader reader)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var sportList = new List<BetSetting>();

            BetSettingHelper.RenderSport(reader, "1", sportList); // Soccer
            BetSettingHelper.RenderSport(reader, "2", sportList); // Basket ball
            BetSettingHelper.RenderSport(reader, "3", sportList); // Football
            BetSettingHelper.RenderSport(reader, "5", sportList); // Tennis
            BetSettingHelper.RenderSport(reader, "8", sportList); // Baseball
            BetSettingHelper.RenderSport(reader, "10", sportList); // Golf
            BetSettingHelper.RenderSport(reader, "11", sportList); // Moto

            // Remove Muay Thai, feature #55015.
            // BetSettingHelper.RenderSport(reader, "44", sportList); // Muay Thai
            BetSettingHelper.RenderSport(reader, "99", sportList); // Other sports

            if (AppSettings.IsShowMixedSport)
            {
                BetSettingHelper.RenderSport(reader, "99MP", sportList);
            }

            BetSettingHelper.RenderSport(reader, "154", sportList); // Horse Racing Fixed Odds

            return sportList;
        }
    }
}