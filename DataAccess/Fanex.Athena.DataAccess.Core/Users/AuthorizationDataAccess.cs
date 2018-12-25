// -----------------------------------------------------------------------
// <copyright file="AuthorizationDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System.Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Common;
    using Session.Interfaces;

    public class AuthorizationDataAccess : BaseDataAccess, IAthorizationDataAccess
    {
        public bool IsCustomerBelonging(int licenseeId, int custId, long transid)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerSelCheckBelonging"))
            {
                db.AddInParameter("licenseeid", DbType.Int32, licenseeId);
                db.AddInParameter("transid", DbType.Int64, transid);
                db.AddInParameter("custid", DbType.Int32, custId);
                db.AddOutParameter("status", DbType.Boolean, sizeof(bool));
                db.ExecuteNonQuery();
                return Converter.ToBool(db.GetParameterValue("status"));
            }
        }
    }
}