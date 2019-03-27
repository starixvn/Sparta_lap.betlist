// -----------------------------------------------------------------------
// <copyright file="CasinoBettingBaseDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Data;
    using Common;
    using Core.Common.Constants;
    using Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;
    using Model.Core.MetaModel.Common;

    public class CoreCasinoBetSettingDataAccess : BaseDataAccess, ICoreCasinoBetSettingDataAccess
    {
        private readonly IUserLogDataAccess _userLogDataAccess;

        public CoreCasinoBetSettingDataAccess(
            IUserLogDataAccess userLogDataAccess)
        {
            _userLogDataAccess = userLogDataAccess;
        }

        public IResultInfo UpdateCasinoStatus(CoreCasinoStatusMetaModel metaModel, int sportId)
        {
            var uspName = "Lrf_CustomerUpdCasinoStatus";

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("userid", metaModel.UserId);
                db.SetParameterValue("custid", metaModel.CustId);
                db.SetParameterValue("sportid", sportId);
                db.SetParameterValue("language", metaModel.Language);
                db.SetParameterValue("ip", metaModel.Ip);
                db.SetParameterValue("status", metaModel.Status);
                db.SetParameterValue("ErrCode", null);
                db.SetParameterValue("ErrMsg", null);

                db.ExecuteNonQueryAsync();

                var errorCode = Converter.ToInt(db.GetParameterValue("ErrCode"));
                var errorMessage = Converter.ToString2(db.GetParameterValue("ErrMsg"));

                var trackingValue1 = GetTrackingValueForUpdateStatus(metaModel, uspName, sportId, metaModel.Status == 1 ? 0 : 1);
                var trackingValue2 = GetTrackingValueForUpdateStatus(metaModel, uspName, sportId, metaModel.Status, errorCode, errorMessage);

                _userLogDataAccess.WriteLog(new UserLogMetaModel
                {
                    UserId = metaModel.UserId,
                    CustID = metaModel.CustId,
                    ActionID = UserLogAction.LiveCasinoBetSetting,
                    SPName = uspName,
                    TrackingValue1 = trackingValue1,
                    TrackingValue2 = trackingValue2
                });

                return new ResultInfo
                {
                    Code = errorCode,
                    Message = errorMessage
                };
            }
        }

        private static string GetTrackingValueForUpdateStatus(
            CoreCasinoStatusMetaModel metaModel,
            string storeProcedureName,
            int sportId,
            int status,
            int errorCode = 0,
            string errorMessage = "")
        {
            var casinoLog = new BetSettingWriteLog { SpName = storeProcedureName };
            casinoLog.TrackingParams.Add(new LogParam { Param = "UserID", Value = metaModel.UserId });
            casinoLog.TrackingParams.Add(new LogParam { Param = "CustID", Value = metaModel.CustId });
            casinoLog.TrackingParams.Add(new LogParam { Param = "SportID", Value = sportId });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Status", Value = status });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Ip", Value = metaModel.Ip });
            casinoLog.TrackingParams.Add(new LogParam { Param = "Lang", Value = metaModel.Language });
            casinoLog.TrackingParams.Add(new LogParam { Param = "RoleID", Value = 1 });
            casinoLog.TrackingParams.Add(new LogParam { Param = "ErrCode", Value = errorCode });
            casinoLog.TrackingParams.Add(new LogParam { Param = "ErrMsg", Value = errorMessage });

            return casinoLog.TrackingValue;
        }
    }
}