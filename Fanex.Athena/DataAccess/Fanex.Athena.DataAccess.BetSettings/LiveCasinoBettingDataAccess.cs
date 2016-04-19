// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Models.DTO.BetSettings;

    public class LiveCasinoBetSettingDataAccess : BaseDataAccess, ILiveCasinoBetSettingDataAccess
    {
        protected const int LiveCasinoActionId = 2037;

        protected const int LiveCasinoSportId = 162;

        public IList<LiveCasinoBetSetting> GetBetSetting(int custId, string language)
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

        public IResultInfo UpdateBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language)
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

        public IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, LiveCasinoSportId, status, ip, language);
        }

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

        private static IResultInfo UpdateCasinoStatus(int userId, int custId, int sportid, int status, string language, string ip)
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

        private static void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2)
        {
            const string UspName = "WriteLog_UserLog_RunSP";

            using (var db = new ObjectDb(UspName))
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

        private LiveCasinoBetSetting GetLiveCasinoLogParam(int custId, int gameTypeIdSelected, string language)
        {
            var liveCasinoList = GetBetSetting(custId, language);
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
    }
}