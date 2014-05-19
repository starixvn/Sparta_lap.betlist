// -----------------------------------------------------------------------
// <copyright file="CurrencyDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Users;
    using Fanex.Athena.Models.MetaModel.Account;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Currency Data access.
    /// </summary>
    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
        /// <summary>
        /// Assign currencies by user.
        /// </summary>
        /// <param name="request">Request value.</param>
        /// <returns>The result value.</returns>
        public IResultInfo AssignCurrenciesByUser(AssignCurrencyMetaModel request)
        {
            const string UspName = "Lrf_CurrencyCustIns";

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, request.UserId);
                    db.AddInParameter("LicenseeID", DbType.Int32, request.LicenseeId);
                    db.AddInParameter("CurrencyXML", DbType.String, request.CurrencyList);
                    db.AddInParameter("ModifyBy", DbType.Int32, request.ModifyBy);
                    db.AddInParameter("LangKey", DbType.String, request.LangKey);
                    db.AddInParameter("IP", DbType.String, request.IP);

                    AddResultParam(db);
                    db.ExecuteNonQuery();

                    return GetResult(db);
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        /// <summary>
        /// Get currencies by user.
        /// </summary>
        /// <param name="licenseeId">Licensee ID value.</param>
        /// <param name="userId">User ID value.</param>
        /// <returns>The result value.</returns>
        public IList<CurrencyUser> GetCurrenciesByUser(int licenseeId, int userId)
        {
            const string UspName = "Lrf_SelCurrencyByLicenseeUser";
            IList<CurrencyUser> currencyList = new List<CurrencyUser>();

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("UserID", DbType.Int32, userId);
                    db.AddInParameter("LicenseeID", DbType.Int32, licenseeId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            currencyList.Add(new CurrencyUser
                            {
                                CurrencyId = reader.ToInt("Currency"),
                                CurrencyName = reader.ToString("CurrencyName"),
                                IsActive = reader.ToBool("IsActive")
                            });
                        }
                    }
                }

                return currencyList;
            }
            catch (System.Exception sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}