// -----------------------------------------------------------------------
// <copyright file="CasinoBettingBaseDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Data;
    using Common;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.BetSettings.DTO;
    using Model.BetSettings.MetaModel;

    public class CoreCasinoBetSettingDataAccess : BaseDataAccess, ICoreCasinoBetSettingDataAccess
    {
        private const int LiveCasinoActionId = 2037;

        public IResultInfo UpdateCasinoStatus(CoreCasinoStatusMetaModel metaModel, int sportId)
        {
            var uspName = "Lrf_CustomerUpdCasinoStatus";

            using (var db = this.GetObjectDb(uspName))
            {
                db.AddInParameter("userid", DbType.Int32, metaModel.UserId);
                db.AddInParameter("custid", DbType.Int32, metaModel.CustId);
                db.AddInParameter("sportid", DbType.Int32, sportId);
                db.AddInParameter("language", DbType.String, metaModel.Language);
                db.AddInParameter("ip", DbType.String, metaModel.Ip);
                db.AddInParameter("status", DbType.Int32, metaModel.Status);
                db.AddOutParameter("ErrCode", DbType.String, 5);
                db.AddOutParameter("ErrMsg", DbType.String, 6000);

                db.ExecuteNonQuery();

                var errorCode = Converter.ToInt(db.GetParameterValue("ErrCode"));
                var errorMessage = Converter.ToString2(db.GetParameterValue("ErrMsg"));

                var trackingValue1 = GetTrackingValueForUpdateStatus(metaModel, uspName, sportId, metaModel.Status == 1 ? 0 : 1);
                var trackingValue2 = GetTrackingValueForUpdateStatus(metaModel, uspName, sportId, metaModel.Status, errorCode, errorMessage);
                WriteLogUpdatingAction(metaModel.UserId, metaModel.CustId, uspName, trackingValue1, trackingValue2);

                return new ResultInfo
                {
                    Code = errorCode,
                    Message = errorMessage
                };
            }
        }

        public void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2)
        {
            const string UspName = "WriteLog_UserLog_RunSP";

            using (var db = this.GetObjectDb(UspName))
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