// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Configuration
{
    using System.Data.SqlClient;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Configuration;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Balance data access.
    /// </summary>
    public class BalanceUpDataAccess : BaseDataAccess, IBalanceUpDataAccess
    {
        /// <summary>
        /// Get balance up date.
        /// </summary>
        /// <returns>The result value.</returns>
        public BalanceUp GetBalanceUpDay()
        {
            const string UspName = "Lrf_SelBalanceUpDay";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var balanceUp = new BalanceUp();
                    using (var reader = db.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            balanceUp.BalanceUpDay = reader.ToDateTime("balanceupday");
                            balanceUp.MoveBetsDay = reader.ToDateTime("movebetsday");
                        }

                        return balanceUp;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}