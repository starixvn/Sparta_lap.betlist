// -----------------------------------------------------------------------
// <copyright file="AuthorizationDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Common;

    public class AuthorizationDataAccess : BaseDataAccess, IAthorizationDataAccess
    {
        public async Task<bool> IsCustomerBelonging(int licenseeId, int custId, long transid)
        {
            using (var db = this.GetObjectDb("Lrf_CustomerSelCheckBelonging"))
            {
                db.SetParameterValue("licenseeid", licenseeId);
                db.SetParameterValue("transid", transid);
                db.SetParameterValue("custid", custId);
                db.SetParameterValue("status", null);
                await db.ExecuteNonQueryAsync().ConfigureAwait(false);
                return Converter.ToBool(db.GetParameterValue("status"));
            }
        }
    }
}