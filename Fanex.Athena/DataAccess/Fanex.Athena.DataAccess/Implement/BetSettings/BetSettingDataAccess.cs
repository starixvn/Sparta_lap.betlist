// -----------------------------------------------------------------------
// <copyright file="BetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.BetSettings;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.BetSettings.AgCasino;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class BetSettingDataAccess : BaseDataAccess, IBetSettingDataAccess
    {
        #region Const

        protected const int GoldDeluxeProductId = 11;
        protected const int MacauProductId = 12;
        protected const int BetTradeProductId = 13;
        protected const int MicroGamingProductId = 14;
        protected const int LiveCasinoActionId = 2037;
        protected const string WriteLogSPName = "WriteLog_UserLog_RunSP";
        protected const int RngCasinoSportId = 251;
        protected const int LiveCasinoSportId = 162;

        #endregion Const

        #region Get bet setting data

        /// <summary>
        /// Gets the third party bet setting.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="productid">The product id.</param>
        /// <returns>The third party bet setting.</returns>
        public ThirdPartyBetSetting GetThirdPartyBetSetting(int custId, int productid)
        {
            var uspName = "Lrf_3rdProductSelCustStatus";
            using (var db = new ObjectDb(uspName))
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
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled"))
                };
            }
        }

        /// <summary>
        /// Get Gold Deluxe setting.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <returns>Gold Deluxe setting.</returns>
        public GoldDeluxeBetSetting GetGoldDeluxeSetting(int custId)
        {
            var uspName = "Lrf_3rdProductSelCustStatus";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddInParameter("ProductId", DbType.Int32, (int)ProductTypes.GoldDeluxe);
                db.AddOutParameter("IsEnabled", DbType.Boolean, 1);
                db.AddOutParameter("IsLineEnabled", DbType.Boolean, 1);
                db.AddOutParameter("Currency", DbType.Int32, 1);

                db.ExecuteNonQuery();

                return new GoldDeluxeBetSetting()
                {
                    IsEnabled = Converter.ToBool(db.GetParameterValue("IsEnabled")),
                    IsLineEnabled = Converter.ToBool(db.GetParameterValue("IsLineEnabled")),
                    CurrencyId = Converter.ToInt(db.GetParameterValue("Currency"))
                };
            }
        }

        /// <summary>
        /// Get Ag Casino setting.
        /// </summary>
        /// <param name="custId">Customer id.</param>
        /// <returns>Ag casino setting.</returns>
        public AgCasinoBetSetting GetAgCasinoSetting(int custId)
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
        /// Get racing bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="isAllowSaToreCurrency">Allow currency.</param>
        /// <returns>Result value.</returns>
        public IList<BetSetting> GetRacingBetSetting(int custId, bool isAllowSaToreCurrency)
        {
            var uspName = "Lrf_CustomerSelEditRB";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return RenderRacingBetSetting(reader, isAllowSaToreCurrency);
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
        /// Get live casino bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="language">Language of licensee.</param>
        /// <returns>The result value.</returns>
        public IList<LiveCasinoBetSetting> GetLiveCasinoBetSetting(int custId, string language)
        {
            var uspName = "TP_Admin_StakeLevel_GetCustStakeLevelMulti";

            var listCasinoBetSetting = new List<LiveCasinoBetSetting>();

            var gameTypeName = GetGameTypeColumn(language);

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);

                var reader = db.ExecuteReader();

                while (reader.Read())
                {
                    var liveCasinoItem = new LiveCasinoBetSetting
                    {
                        GameType = reader.ToString(gameTypeName),
                        GameTypeId = reader.ToInt("GameTypeId"),
                        StakeLevelId = reader.ToInt("StakeLevelID"),
                        StakeLevelName = reader.ToString("StakeLevelName"),
                        IsCheck = reader.ToBool("IsChecked")
                    };

                    listCasinoBetSetting.Add(liveCasinoItem);
                }

                return listCasinoBetSetting;
            }
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

        #region Gold Deluxe

        /// <summary>
        /// Update Gold Deluxe Bet Setting.
        /// </summary>
        /// <param name="data">Setting data.</param>
        /// <param name="group">Bet setting group.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateGoldDeluxeBetSetting(CommonBetSetting data, string group)
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

        /// <summary>
        /// Update Gold Deluxe status.
        /// </summary>
        /// <param name="userId">User identification.</param>
        /// <param name="custId">Customer id.</param>
        /// <param name="language">Language code.</param>
        /// <param name="ip">IP address string.</param>
        /// <param name="isEnabled">Status value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateGoldDeluxeStatus(int userId, int custId, string language, string ip, bool isEnabled)
        {
            var uspName = "Lrf_3rdProductUpdCustStatus";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("productid", DbType.Int32, (int)ProductTypes.GoldDeluxe);
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

        #endregion Gold Deluxe

        #region Macau

        /// <summary>
        /// Updates the macau status.
        /// </summary>
        /// <param name="productId">The product ID.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="language">The language.</param>
        /// <param name="ip">The host address.</param>
        /// <param name="isEnabled">The macau status.</param>
        /// <returns>The Result Info.</returns>
        public IResultInfo UpdateThirdPartyProductStatus(
            int productId,
            int userId,
            int custId,
            string language,
            string ip,
            bool isEnabled)
        {
            var uspName = "Lrf_3rdProductUpdCustStatus";

            using (var db = new ObjectDb(uspName))
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

        #endregion Macau

        #region AG Casino

        /// <summary>
        /// Update Ag Casino Bet Setting.
        /// </summary>
        /// <param name="data">Setting data.</param>
        /// <param name="group">Bet setting group.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateAgCasinoBetSetting(CommonBetSetting data, string group)
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

        /// <summary>
        /// Update Ag Casino status.
        /// </summary>
        /// <param name="userId">User identification.</param>
        /// <param name="custId">Customer id.</param>
        /// <param name="language">Language code.</param>
        /// <param name="ip">IP address string.</param>
        /// <param name="isEnabled">Status value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateAgCasinoStatus(int userId, int custId, string language, string ip, bool isEnabled)
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

        #endregion AG Casino

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

        #region Racing

        /// <summary>
        /// Update racing bet setting.
        /// </summary>
        /// <param name="racingBetSettingList">Racing bet setting list.</param>
        /// <returns>The result value.</returns>
        public IDictionary<string, string> UpdateRacingBetSetting(Dictionary<string, RacingBetSetting> racingBetSettingList)
        {
            var code = string.Empty;
            var message = string.Empty;

            var result = new Dictionary<string, string>();

            foreach (var racingBetSetting in racingBetSettingList.Where(r => r.Value.IsDataChange))
            {
                switch (racingBetSetting.Key)
                {
                    case "151_31":
                        UpdateRacing15131(racingBetSetting.Value);
                        break;

                    case "151_41":
                        UpdateRacing15141(racingBetSetting.Value);
                        break;

                    case "151_51":
                        UpdateRacing15151(racingBetSetting.Value);
                        break;

                    case "152_41":
                        UpdateRacing15241(racingBetSetting.Value);
                        break;

                    case "152_51":
                        UpdateRacing15251(racingBetSetting.Value);
                        break;

                    case "153_51":
                        UpdateRacing15351(racingBetSetting.Value);
                        break;
                }

                if (racingBetSetting.Value.ResultInfo.Code == 0)
                {
                    continue;
                }

                code += ";" + racingBetSetting.Value.ResultInfo.Code;
                message += ";" + racingBetSetting.Value.ResultInfo.Message;
            }

            result.Add("Code", code);
            result.Add("Message", message);

            return result;
        }

        #region Update racing types

        public void UpdateRacing15131(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_31";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15141(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15151(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15241(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15251(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15351(CommonBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet153_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        /// <summary>
        /// Racing updating process.
        /// </summary>
        /// <param name="uspName">USP name value.</param>
        /// <param name="racingBetSetting">Racing bet setting.</param>
        public void RacingUpdatingProcess(string uspName, CommonBetSetting racingBetSetting)
        {
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, racingBetSetting.CustId);
                    db.AddInParameter("userid", DbType.Int32, racingBetSetting.UserId);
                    db.AddInParameter("minbet", DbType.Decimal, racingBetSetting.MinBet);
                    db.AddInParameter("maxbet", DbType.Decimal, racingBetSetting.MaxBet);
                    db.AddInParameter("maxpermatch", DbType.Decimal, racingBetSetting.MaxPerMatch);
                    db.AddInParameter("language", DbType.String, racingBetSetting.Language);
                    db.AddInParameter("ip", DbType.String, racingBetSetting.Ip);
                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    var result = GetResult(db);

                    racingBetSetting.ResultInfo = (ResultInfo)result;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, "No params");
            }
        }

        #endregion Update racing types

        #endregion Racing

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

        #region Live Casino

        /// <summary>
        /// Update live casino bet setting.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="param">Parameter value.</param>
        /// <param name="ip">The IP value.</param>
        /// <param name="language">Language value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateLiveCasinoBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language)
        {
            var uspName = "TP_User_CustGameType_UpdStakeLevel";

            // Tracking value before update (Tracking value 2)
            var oldParam = GetLiveCasinoLogParam(custId, param.GameTypeId, language);
            var trackingValue2 = GetTrackingValue(uspName, custId, oldParam, ip, language);

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("gametypeid", DbType.Int32, param.GameTypeId);
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("stakelevellist", DbType.Int32, param.StakeLevelId);
                db.AddInParameter("roleid", DbType.Int32, 1);
                db.AddOutParameter("ErrCode", DbType.String, 5);
                db.AddOutParameter("ErrMsg", DbType.String, 6000);

                db.ExecuteNonQuery();

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");

                var result = new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString2(errorMessage)
                };

                // Tracking value after update (Tracking value 1)
                var newParam = GetLiveCasinoLogParam(custId, param.GameTypeId, language);
                var trackingValue1 = GetTrackingValue(uspName, custId, newParam, ip, language, result);

                // Write Log
                WriteLogUpdatingAction(userId, custId, uspName, trackingValue1, trackingValue2);

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString2(errorMessage)
                };
            }
        }

        public IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, LiveCasinoSportId, status, ip, language);
        }

        public IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, RngCasinoSportId, status, ip, language);
        }

        #endregion Live Casino

        #endregion Update bet setting

        #region Live Casino Utility

        /// <summary>
        /// Get game type name.
        /// </summary>
        /// <param name="language">Language of licensee.</param>
        /// <returns>Game type name.</returns>
        private static string GetGameTypeColumn(string language)
        {
            string gameTypeName = "GameTypeName_En";

            switch (language)
            {
                case "zh-TW":
                    gameTypeName = "GameTypeName_Ct";
                    break;

                case "zh-CN":
                case "zh-Hans":
                    gameTypeName = "GameTypeName_Cs";
                    break;

                case "ko-KR":
                    gameTypeName = "GameTypeName_Ko";
                    break;
            }

            return gameTypeName;
        }

        #endregion Live Casino Utility

        #region Write Log

        #region Live Casino

        /// <summary>
        /// Get tracking value for update casino status.
        /// </summary>
        /// <param name="storeProcedureName">Store procedure name value.</param>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="sportid">Sport ID value.</param>
        /// <param name="status">The Status value.</param>
        /// <param name="ip">The IP value.</param>
        /// <param name="language">Language value.</param>
        /// <param name="resultInfo">Result Information value.</param>
        /// <returns>The result value.</returns>
        private static string GetTrackingValueForUpdateStatus(
            string storeProcedureName,
            int userId,
            int custId,
            int sportid,
            int status,
            string ip,
            string language,
            IResultInfo resultInfo = null)
        {
            var casinoLog = new BetSettingWriteLog { SpName = storeProcedureName };
            casinoLog.TrackingParams.Add(new LogParam { Param = "UserID", Value = userId });
            casinoLog.TrackingParams.Add(new LogParam { Param = "CustID", Value = custId });
            casinoLog.TrackingParams.Add(new LogParam { Param = "SportID", Value = sportid });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Status", Value = status });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Ip", Value = ip });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Lang", Value = language });
            casinoLog.TrackingParams.Add(new LogParam { Param = "RoleID", Value = 1 });
            casinoLog.TrackingParams.Add(new LogParam { Param = "ErrCode", Value = resultInfo != null ? resultInfo.Code : 0 });
            casinoLog.TrackingParams.Add(new LogParam { Param = "ErrMsg", Value = resultInfo != null ? resultInfo.Message : string.Empty });

            return casinoLog.TrackingValue;
        }

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

        /// <summary>
        /// Get tracking value.
        /// </summary>
        /// <param name="storeProcedureName">Store procedure name value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="param">Parameter value.</param>
        /// <param name="ip">The IP value.</param>
        /// <param name="language">Language value.</param>
        /// <param name="resultInfo">Result Information value.</param>
        /// <returns>The result value.</returns>
        private static string GetTrackingValue(
            string storeProcedureName,
            int custId,
            LiveCasinoBetSetting param,
            string ip,
            string language,
            IResultInfo resultInfo = null)
        {
            var liveCasinoLog = new BetSettingWriteLog { SpName = storeProcedureName };
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "CustID", Value = custId });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "GameTypeID", Value = param.GameTypeId });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "BetTypeID", Value = param.BetTypeId });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "StakeLevel", Value = param.StakeLevelName });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "Ip", Value = ip });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "Lang", Value = language });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "RoleID", Value = 1 });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "ErrCode", Value = resultInfo != null ? resultInfo.Code : 0 });
            liveCasinoLog.TrackingParams.Add(new LogParam { Param = "ErrMsg", Value = resultInfo != null ? resultInfo.Message : string.Empty });

            return liveCasinoLog.TrackingValue;
        }

        /// <summary>
        /// Update live casino and RNG casino status.
        /// </summary>
        /// <param name="userId">The User ID.</param>
        /// <param name="custId">The Customer ID.</param>
        /// <param name="sportid">The Sport ID.</param>
        /// <param name="status">The Game status.</param>
        /// <param name="language">The Language value.</param>
        /// <param name="ip">The IP Value.</param>
        /// <returns>Code and Message.</returns>
        private IResultInfo UpdateCasinoStatus(int userId, int custId, int sportid, int status, string language, string ip)
        {
            var uspName = "Lrf_CustomerUpdCasinoStatus";

            var trackingValue1 = GetTrackingValueForUpdateStatus(uspName, userId, custId, sportid, status == 1 ? 0 : 1, ip, language);
            var trackingValue2 = GetTrackingValueForUpdateStatus(uspName, userId, custId, sportid, status, ip, language);

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("userid", DbType.Int32, userId);
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddInParameter("sportid", DbType.Int32, sportid);
                db.AddInParameter("language", DbType.String, language);
                db.AddInParameter("ip", DbType.String, ip);
                db.AddInParameter("status", DbType.Int32, status);
                db.AddOutParameter("ErrCode", DbType.String, 5);
                db.AddOutParameter("ErrMsg", DbType.String, 6000);

                db.ExecuteNonQuery();

                var errorCode = db.GetParameterValue("ErrCode");
                var errorMessage = db.GetParameterValue("ErrMsg");
                WriteLogUpdatingAction(userId, custId, uspName, trackingValue1, trackingValue2);

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString2(errorMessage)
                };
            }
        }

        /// <summary>
        /// Write log live casino.
        /// </summary>
        /// <param name="userId">User ID value.</param>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="storeProcedureName">Store procedure name value.</param>
        /// <param name="trackingValue1">Tracking value 1 value.</param>
        /// <param name="trackingValue2">Tracking value 2 value.</param>
        private void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2)
        {
            var uspName = WriteLogSPName;

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("UserID", DbType.Int32, userId);
                db.AddInParameter("CustID", DbType.Int32, custId);
                db.AddInParameter("SPName", DbType.String, storeProcedureName);
                db.AddInParameter("ActionID", DbType.Int32, LiveCasinoActionId);
                db.AddInParameter("TrackingValue", DbType.String, trackingValue1);
                db.AddInParameter("TrackingValue2", DbType.String, trackingValue2);
                db.AddInParameter("Duration", DbType.Int32, 0);

                db.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Get live casino log parameter.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <param name="gameTypeIdSelected">Game type ID selected value.</param>
        /// <param name="language">Language of licensee.</param>
        /// <returns>The result value.</returns>
        private LiveCasinoBetSetting GetLiveCasinoLogParam(int custId, int gameTypeIdSelected, string language)
        {
            var liveCasinoList = GetLiveCasinoBetSetting(custId, language);
            var gameType = liveCasinoList.Where(x => x.GameTypeId == gameTypeIdSelected && x.IsCheck).Select(x => x.GameType).FirstOrDefault();
            var gameTypeId = gameTypeIdSelected;
            var stakeLevelName = liveCasinoList
                .Where(x => x.GameTypeId == gameTypeIdSelected && x.IsCheck).Select(x => x.StakeLevelName).FirstOrDefault();
            var stakeLevelId = liveCasinoList.Where(x => x.GameTypeId == gameTypeIdSelected && x.IsCheck).Select(x => x.StakeLevelId).FirstOrDefault();
            var betTypeId = liveCasinoList.Where(x => x.GameTypeId == gameTypeIdSelected && x.IsCheck).Select(x => x.BetTypeId).FirstOrDefault();

            return new LiveCasinoBetSetting
            {
                GameType = gameType,
                GameTypeId = gameTypeId,
                StakeLevelId = stakeLevelId,
                StakeLevelName = stakeLevelName,
                BetTypeId = betTypeId
            };
        }

        #endregion Live Casino

        #endregion Write Log

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

        #region Racing

        /// <summary>
        /// Render racing bet setting.
        /// </summary>
        /// <param name="reader">Racing table value.</param>
        /// <param name="isAllowSaToteCurrency">Allow Tote currency value.</param>
        /// <returns>The result value.</returns>
        private IList<BetSetting> RenderRacingBetSetting(IDataReader reader, bool isAllowSaToteCurrency)
        {
            if (reader.FieldCount == 0)
            {
                return null;
            }

            var racingList = new List<BetSetting>();

            BetSettingHelper.RenderSport(reader, "151_41", racingList); // Horse racing UK

            if (isAllowSaToteCurrency)
            {
                BetSettingHelper.RenderSport(reader, "151_31", racingList); // Horse racing SA
            }

            BetSettingHelper.RenderSport(reader, "152_51", racingList); // Greyhound Aust
            BetSettingHelper.RenderSport(reader, "153_51", racingList); // Harness Aust

            return racingList;
        }

        #endregion Racing

        #endregion Utility
    }
}