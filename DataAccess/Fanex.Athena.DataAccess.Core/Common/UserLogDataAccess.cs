// -----------------------------------------------------------------------
// <copyright file="LeagueDataAccess.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Threading.Tasks;
    using Interfaces;
    using Model.Core.MetaModel.Common;

    public class UserLogDataAccess : BaseDataAccess, IUserLogDataAccess
    {
        public async Task WriteLog(UserLogMetaModel metaModel)
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

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
        }
    }
}