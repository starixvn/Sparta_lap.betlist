// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Data;
    using Extension.Utility;
    using Interfaces;
    using Model.Core.MetaModel.Common;
    using Session.Interfaces;

    public class UserLogDataAccess : BaseDataAccess, IUserLogDataAccess
    {
        public void WriteLog(UserLogMetaModel metaModel)
        {
            const string UspName = "WriteLog_UserLog_RunSP";

            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                db.AddInParameter("CustID", DbType.Int32, metaModel.CustID);
                db.AddInParameter("SPName", DbType.String, metaModel.SPName);
                db.AddInParameter("ActionID", DbType.Int32, metaModel.ActionID);
                db.AddInParameter("Doing", DbType.String, metaModel.Doing);
                db.AddInParameter("TrackingValue", DbType.String, metaModel.TrackingValue1);
                db.AddInParameter("TrackingValue2", DbType.String, metaModel.TrackingValue2);
                db.AddInParameter("Duration", DbType.Int32, 0);

                db.ExecuteNonQuery();
            }
        }
    }
}