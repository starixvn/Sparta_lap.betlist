// -----------------------------------------------------------------------
// <copyright file="UserDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Interfaces;
    using Model.Core.DTO.OneAuth;

    public class OneAuthDataAccess : BaseDataAccess, IOneAuthDataAccess
    {
        public OneAuthUserInfo GetUserInfo(int userSEQ, string vendorId)
        {
            const string UspName = "Lrf_GetUserLevelInfo";
            using (var db = this.GetObjectDb(UspName))
            {
                db.AddInParameter("VendorID", DbType.String, vendorId);
                db.AddInParameter("UserSEQ", DbType.Int32, userSEQ);
                db.AddOutParameter("UserLevel", DbType.Byte, 0);
                db.AddOutParameter("SubsidiaryEnabled", DbType.Boolean, 0);
                db.AddOutParameter("OperatorID", DbType.String, 50);

                var result = new OneAuthUserInfo();
                db.ExecuteNonQuery();

                result.UserLevel = Convert.ToByte(db.GetParameterValue("UserLevel"));
                result.IsEnabledSubsidiary = Convert.ToBoolean(db.GetParameterValue("SubsidiaryEnabled"));
                result.OperatorID = Convert.ToString(db.GetParameterValue("OperatorID"));
                return result;
            }
        }
    }
}