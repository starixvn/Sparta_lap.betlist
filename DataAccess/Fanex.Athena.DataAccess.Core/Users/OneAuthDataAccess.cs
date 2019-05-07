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
                db.SetParameterValue("VendorID", vendorId);
                db.SetParameterValue("UserSEQ", userSEQ);
                db.SetParameterValue("UserLevel", null);
                db.SetParameterValue("SubsidiaryEnabled", null);
                db.SetParameterValue("OperatorID", null);

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