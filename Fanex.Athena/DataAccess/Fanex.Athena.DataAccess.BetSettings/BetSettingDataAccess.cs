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
        #region Get bet setting data

        /// <summary>
        /// Get sport book bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="esportSetting">E-sport Setting.</param>
        /// <returns>List result.</returns>
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

        /// <summary>
        /// Get virtual sport bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>The result value.</returns>
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

        /// <summary>
        /// Get Keno bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>List result.</returns>
        public IList<BetSetting> GetKenoBetSettings(int custId)
        {
            var uspName = "Lrf_CustomerSelEditKN";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    return RenderKenoBetSetting(reader);
                }
            }
        }

        #endregion Get bet setting data

        #region Update bet setting

        #region Sportsbook

        /// <summary>
        /// Update sport book bet setting.
        /// </summary>
        /// <param name="data">The common setting.</param>
        /// <param name="setting">The customer setting.</param>
        /// <returns>The result value.</returns>
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

        /// <summary>
        /// Update E-sport Status.
        /// </summary>
        /// <param name="setting">Setting value.</param>
        /// <param name="isDisabled">Is disable.</param>
        /// <returns>Result information.</returns>
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

        #endregion Sportsbook

        #region Keno

        /// <summary>
        /// Update Keno bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="language">The language code. </param>
        /// <param name="ip">The IP address.</param>
        /// <param name="xmlData">The XML parameters.</param>
        /// <returns>The result info.</returns>
        public IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, string xmlData)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBetKN";

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

        #endregion Keno

        #region Virtual sports

        /// <summary>
        /// Update virtual sport bet setting.
        /// </summary>
        /// <param name="custId">The customer id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="language">The language code. </param>
        /// <param name="ip">The IP address.</param>
        /// <param name="xmlData">The XML parameters.</param>
        /// <returns>The result info.</returns>
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

        #endregion Virtual sports

        #endregion Update bet setting

        /// <summary>
        /// Render Keno bet setting.
        /// </summary>
        /// <param name="reader">Data table value.</param>
        /// <returns>The result value.</returns>
        private static IList<BetSetting> RenderKenoBetSetting(IDataReader reader)
        {
            var betSettings = new List<BetSetting>();
            while (reader.Read())
            {
                var gameTypeId = Converter.ToString2(reader["GameTypeID"]);
                var sport = new BetSetting
                {
                    MaxBet = Converter.ToDecimal(reader["MaxBet"]),
                    MinBet = Converter.ToDecimal(reader["MinBet"]),
                    MaxPerMatchMax = Converter.ToDecimal(reader["MaxPerMatch_Max"]),
                    MaxPerMatch = Converter.ToDecimal(reader["MaxPerMatch"]),
                    BetValueMax = Converter.ToDecimal(reader["MaxBet_Min"]),
                    BetValueMin = Converter.ToDecimal(reader["MinBet_Max"]),
                    BetSettingType = CacheRef.GetBettype(Converter.ToString2(reader["IBCBetTypeID"])),
                    SportId = gameTypeId
                };

                betSettings.Add(sport);
            }

            return betSettings;
        }

        #region Utility

        #region Sport Book

        /// <summary>
        /// Render Sport book bet setting.
        /// </summary>
        /// <param name="reader">Data reader.</param>
        /// <returns>The result value.</returns>
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

        #endregion Sport Book

        #endregion Utility
    }
}