// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration
{
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Configuration;
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
                using (var db = this.GetObjectDb(UspName))
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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}