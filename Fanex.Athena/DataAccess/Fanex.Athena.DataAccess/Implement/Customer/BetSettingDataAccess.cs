// -----------------------------------------------------------------------
// <copyright file="BetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Customer
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Customer;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Customer.BetSetting;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     Bet setting data access.
    /// </summary>
    public class BetSettingDataAccess : BaseDataAccess, IBetSettingDataAccess
    {
        #region Const

        protected readonly int LiveCasinoActionId = 2037;
        protected readonly string WriteLogSpName = "WriteLog_UserLog_RunSP";
        protected readonly int RngCasinoSportId = 251;
        protected readonly int LiveCasinoSportId = 162;

        #endregion Const

        #region Field

        private static Dictionary<string, string> _sportNameMap;
        private bool _isNumberGame;

        #endregion Field

        #region Get bet setting data

        /// <summary>
        /// Get sport book bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>List result.</returns>
        public IList<BetSetting> GetSportBookBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditCustomer";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                var sportBookDataSet = db.ExecuteDataSet();
                if (sportBookDataSet != null)
                {
                    return RenderSportBookBetSetting(sportBookDataSet.Tables[0]);
                }

                return new List<BetSetting>();
            }
        }

        /// <summary>
        /// Get number game bet setting.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>Result value.</returns>
        public IList<BetSetting> GetNumberGameBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditNG";

            // Set isNumberGame = true
            this._isNumberGame = true;

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);

                var numberGameDataSet = db.ExecuteDataSet();
                if (numberGameDataSet != null)
                {
                    return RenderNumberGameBetSetting(numberGameDataSet.Tables[0]);
                }

                return new List<BetSetting>();
            }
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

                var racingDataSet = db.ExecuteDataSet();
                if (racingDataSet != null)
                {
                    return RenderRacingBetSetting(racingDataSet.Tables[0], isAllowSaToreCurrency);
                }

                return new List<BetSetting>();
            }
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
        public IList<BetSetting> GetKenoBetSetting(int custId)
        {
            var uspName = "Lrf_CustomerSelEditKN";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("CustID", DbType.Int32, custId);
                var dataSet = db.ExecuteDataSet();
                if (dataSet != null)
                {
                    return RenderKenoBetSetting(dataSet.Tables[0]);
                }

                return new List<BetSetting>();
            }
        }

        #endregion Get bet setting data

        #region Update bet setting

        #region Sportsbook

        /// <summary>
        /// Update sport book bet setting.
        /// </summary>
        /// <param name="common">The common setting.</param>
        /// <param name="setting">The customer setting.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateSportsBookBetSetting(CommonBetSetting common, IDictionary<string, object> setting)
        {
            var uspName = "Lrf_CustomerUpdMember_All";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("licenseeid", DbType.Int32, common.LicenseeId);
                db.AddInParameter("ip", DbType.String, common.Ip);
                db.AddInParameter("language", DbType.String, common.Language);
                db.AddInParameter("userid", DbType.Int32, common.UserId);
                db.AddInParameter("custid", DbType.Int32, common.CustId);
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

        public void UpdateRacing15131(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_31";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15141(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15151(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet151_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15241(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_41";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15251(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet152_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        public void UpdateRacing15351(RacingBetSetting racingBetSetting)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet153_51";

            RacingUpdatingProcess(uspName, racingBetSetting);
        }

        /// <summary>
        /// Racing updating process.
        /// </summary>
        /// <param name="uspName">USP name value.</param>
        /// <param name="racingBetSetting">Racing bet setting.</param>
        public void RacingUpdatingProcess(string uspName, RacingBetSetting racingBetSetting)
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
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, "No params");
            }
        }

        #endregion Update racing types

        #endregion Racing

        #region Number game

        /// <summary>
        /// Update number game bet setting.
        /// </summary>
        /// <param name="param">Parameter value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo UpdateNumbetGameBetSetting(NumberGameBetSetting param)
        {
            var uspName = "Lrf_CustomerUpdMember_MinMaxBet161";

            using (var db = new ObjectDb(uspName))
            {
                db.AddInParameter("custid", DbType.Int32, param.CustId);
                db.AddInParameter("userid", DbType.Int32, param.UserId);
                db.AddInParameter("minbetv161", DbType.Decimal, param.MinBet);
                db.AddInParameter("maxbetv161", DbType.Decimal, param.MaxBet);
                db.AddInParameter("maxpermatchv161", DbType.Decimal, param.MaxPerMatch);
                db.AddInParameter("maxperballv161", DbType.Decimal, param.MaxPerBall);
                db.AddInParameter("language", DbType.String, param.Language);
                db.AddInParameter("ip", DbType.String, param.Ip);
                AddResultParam(db);
                db.ExecuteNonQuery();

                return GetResult(db);
            }
        }

        #endregion Number game

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

        #endregion Live Casino

        #endregion Update bet setting

        public IResultInfo UpdateLiveCasinoStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, this.LiveCasinoSportId, status, ip, language);
        }

        public IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, this.RngCasinoSportId, status, ip, language);
        }

        #region Get Sport Name

        /// <summary>
        /// Get sport name.
        /// </summary>
        /// <param name="sportId">Sport ID value.</param>
        /// <returns>The result value.</returns>
        private static string GetSportName(string sportId)
        {
            if (_sportNameMap == null)
            {
                _sportNameMap = new Dictionary<string, string>
                    {
                        { "99MP", ViewResources.MixSportsParlay },
                        { "151_31", ViewResources.HorseRacingSATote },
                        { "151_41", ViewResources.HorseRacingUK_IrishTote },
                        { "151_51", ViewResources.HorseRacingAustTote },
                        { "152_41", ViewResources.GreyhoundUK_IrishTote },
                        { "152_51", ViewResources.GreyhoundAustTote },
                        { "153_51", ViewResources.HarnessAustTote },
                    };
            }

            if (_sportNameMap.ContainsKey(sportId))
            {
                return _sportNameMap[sportId];
            }

            return ViewResources.OtherSport;
        }

        #endregion Get Sport Name

        #region Live Casino Utility

        /// <summary>
        /// Get game type name.
        /// </summary>
        /// <param name="language">Language of licensee.</param>
        /// <returns>Game type name.</returns>
        private static string GetGameTypeColumn(string language)
        {
            string gameTypeName = string.Empty;

            switch (language)
            {
                case "zh-TW":
                    gameTypeName = "GameTypeName_Ct";
                    break;
                case "zh-CN":
                    gameTypeName = "GameTypeName_Cs";
                    break;
                case "ko-KR":
                    gameTypeName = "GameTypeName_Ko";
                    break;
                default:
                    gameTypeName = "GameTypeName_En";
                    break;
            }

            return gameTypeName;
        }

        #endregion

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
        /// <param name="table">Data table value.</param>
        /// <returns>The result value.</returns>
        private static IList<BetSetting> RenderKenoBetSetting(DataTable table)
        {
            var betSettings = new List<BetSetting>();
            foreach (DataRow row in table.Rows)
            {
                var gameTypeId = Converter.ToString2(row["GameTypeID"]);
                var sport = new BetSetting
                {
                    MaxBet = Converter.ToDecimal(row["MaxBet"]),
                    MinBet = Converter.ToDecimal(row["MinBet"]),
                    MaxPerMatchMax = Converter.ToDecimal(row["MaxPerMatch_Max"]),
                    MaxPerMatch = Converter.ToDecimal(row["MaxPerMatch"]),
                    BetValueMax = Converter.ToDecimal(row["MaxBet_Min"]),
                    BetValueMin = Converter.ToDecimal(row["MinBet_Max"]),
                    BetSettingType = CacheRef.GetBettype(Converter.ToString2(row["IBCBetTypeID"])),
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
            var uspName = WriteLogSpName;

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
        /// <param name="sportBookTable">Sport book table value.</param>
        /// <returns>The result value.</returns>
        private IList<BetSetting> RenderSportBookBetSetting(DataTable sportBookTable)
        {
            if (sportBookTable.Rows.Count == 0)
            {
                return null;
            }

            var sportList = new List<BetSetting>();
            var row = sportBookTable.Rows[0];

            RenderSport(row, "1", sportList); // Soccer
            RenderSport(row, "2", sportList); // Basket ball
            RenderSport(row, "3", sportList); // Football
            RenderSport(row, "5", sportList); // Tennis
            RenderSport(row, "8", sportList); // Baseball
            RenderSport(row, "10", sportList); // Golf
            RenderSport(row, "11", sportList); // Moto
            RenderSport(row, "99", sportList); // Other sports

            if (AppSettings.IsShowMixedSport)
            {
                RenderSport(row, "99MP", sportList);
            }

            RenderSport(row, "154", sportList); // Horse Racing Fixed Odds

            return sportList;
        }

        #endregion Sport Book

        #region Number Game

        /// <summary>
        /// Render number game bet setting.
        /// </summary>
        /// <param name="numberGameTable">Number game table value.</param>
        /// <returns>The result value.</returns>
        private IList<BetSetting> RenderNumberGameBetSetting(DataTable numberGameTable)
        {
            if (numberGameTable.Rows.Count == 0)
            {
                return null;
            }

            var numberGameList = new List<BetSetting>();
            var row = numberGameTable.Rows[0];

            RenderSport(row, "161", numberGameList); // Soccer

            return numberGameList;
        }

        #endregion Number Game

        #region Racing

        /// <summary>
        /// Render racing bet setting.
        /// </summary>
        /// <param name="racingTable">Racing table value.</param>
        /// <param name="isAllowSaToteCurrency">Allow Tote currency value.</param>
        /// <returns>The result value.</returns>
        private IList<BetSetting> RenderRacingBetSetting(DataTable racingTable, bool isAllowSaToteCurrency)
        {
            if (racingTable.Rows.Count == 0)
            {
                return null;
            }

            var racingList = new List<BetSetting>();
            var row = racingTable.Rows[0];

            RenderSport(row, "151_41", racingList); // Horse racing UK

            if (isAllowSaToteCurrency)
            {
                RenderSport(row, "151_31", racingList); // Horse racing SA
            }

            RenderSport(row, "151_51", racingList); // Horse racing Aust
            RenderSport(row, "152_41", racingList); // Greyhound UK
            RenderSport(row, "152_51", racingList); // Greyhound Aust
            RenderSport(row, "153_51", racingList); // Harness Aust

            return racingList;
        }

        #endregion Racing

        #region Get Bet Setting Details

        /// <summary>
        /// Render sport.
        /// </summary>
        /// <param name="row">The row value.</param>
        /// <param name="sportId">Sport ID value.</param>
        /// <param name="resultList">Result list value.</param>
        private void RenderSport(DataRow row, string sportId, List<BetSetting> resultList)
        {
            var sportSuffix = sportId != "1" ? "v" + sportId : string.Empty;
            var sportName = CacheRef.GetSportName(sportId);

            sportName = string.IsNullOrEmpty(sportName) ? GetSportName(sportId) : sportName;

            var sport = new BetSetting
            {
                MaxBet = Converter.ToDecimal(row["maxbet" + sportSuffix]),
                MinBet = Converter.ToDecimal(row["minbet" + sportSuffix]),
                MaxPerMatch = Converter.ToDecimal(row["MaxPerMatch" + sportSuffix]),
                MaxPerMatchMax = Converter.ToDecimal(row["MaxPerMatch" + sportSuffix + "_Max"]),
                MaxPerMatchMin = Converter.ToDecimal(row["MaxPerMatch" + sportSuffix + "_Min"]),
                BetValueMax = Converter.ToDecimal(row["MaxBet" + sportSuffix + "_Min"]),
                BetValueMin = Converter.ToDecimal(row["MinBet" + sportSuffix + "_Max"]),
                BetSettingType = sportName,
                SportId = sportId
            };

            if (_isNumberGame)
            {
                sport.MaxPerBall = Converter.ToDecimal(row["maxperball" + sportSuffix]);
                sport.MaxPerBallMax = Converter.ToDecimal(row["maxperball" + sportSuffix + "_Max"]);
            }

            resultList.Add(sport);
        }

        #endregion Get Bet Setting Details

        #endregion Utility
    }
}