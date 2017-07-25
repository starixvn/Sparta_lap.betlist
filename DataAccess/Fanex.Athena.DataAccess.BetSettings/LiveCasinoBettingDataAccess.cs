// -----------------------------------------------------------------------
// <copyright file="LiveCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Interfaces;
    using Model.BetSettings.DTO;

    public class LiveCasinoBetSettingDataAccess : CasinoBettingBaseDataAccess, ILiveCasinoBetSettingDataAccess
    {
        private const int LiveCasinoSportId = 162;

        public IList<LiveCasinoBetSetting> GetBetSetting(int custId, string language)
        {
            const string UspName = "TP_Admin_StakeLevel_GetCustStakeLevelMulti";

            var listCasinoBetSetting = new List<LiveCasinoBetSetting>();

            var gameTypeName = GetGameTypeColumn(language);

            using (var db = this.GetObjectDb(UspName))
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

                return listCasinoBetSetting.OrderBy(x => x.GameTypeId).ToList();
            }
        }

        public IResultInfo UpdateBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language)
        {
            const string UspName = "TP_User_CustGameType_UpdStakeLevel";

            // Tracking value before update (Tracking value 2)
            var oldParam = GetLiveCasinoLogParam(custId, param.GameTypeId, language);
            var trackingValue2 = GetTrackingValue(UspName, custId, oldParam, ip, language);

            using (var db = this.GetObjectDb(UspName))
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
                var trackingValue1 = GetTrackingValue(UspName, custId, newParam, ip, language, result);

                WriteLogUpdatingAction(userId, custId, UspName, trackingValue1, trackingValue2);

                return new ResultInfo
                {
                    Code = Converter.ToInt(errorCode),
                    Message = Converter.ToString2(errorMessage)
                };
            }
        }

        public IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, LiveCasinoSportId, status, language, ip);
        }

        private static string GetGameTypeColumn(string language)
        {
            var gameTypeName = "GameTypeName_En";

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