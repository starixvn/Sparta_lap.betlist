// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Data;
    using Interfaces;
    using Model.Core.MetaModel.Common;

    public class UserLogDataAccess : BaseDataAccess, IUserLogDataAccess
    {
        public void WriteLog(UserLogMetaModel metaModel)
        {
            const string UspName = "WriteLog_UserLog_RunSP";

            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("UserID", metaModel.UserId);
                db.SetParameterValue("CustID", metaModel.CustID);
                db.SetParameterValue("SPName", metaModel.SPName);
                db.SetParameterValue("ActionID", metaModel.ActionID);
                db.SetParameterValue("Doing", metaModel.Doing);
                db.SetParameterValue("TrackingValue", metaModel.TrackingValue1);
                db.SetParameterValue("TrackingValue2", metaModel.TrackingValue2);
                db.SetParameterValue("Duration", 0);

                db.ExecuteNonQuery();
            }
        }
    }
}