// -----------------------------------------------------------------------
// <copyright file="AuthorizationDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users
{
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Common;
    using Fanex.Data;

    /// <summary>
    /// Authorization data access.
    /// </summary>
    public class AuthorizationDataAccess : BaseDataAccess, IAthorizationDataAccess
    {
        /// <summary>
        /// Customer belong.
        /// </summary>
        /// <param name="licenseeId">Licensee ID.</param>
        /// <param name="custId">Customer ID.</param>
        /// <param name="transid">Transaction ID.</param>
        /// <returns>Boolean value.</returns>
        public bool IsCustomerBelonging(int licenseeId, int custId, long transid)
        {
            using (var db = new ObjectDb("Lrf_CustomerSelCheckBelonging"))
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