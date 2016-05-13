// -----------------------------------------------------------------------
// <copyright file="ExChangeRateDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Data;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Exchange rate data access.
    /// </summary>
    public class ExChangeRateDataAccess : IExChangeRateDataAccess
    {
        /// <summary>
        /// Get exchange rate.
        /// </summary>
        /// <param name="currenyCode">Currency value.</param>
        /// <param name="datetime">Date time value.</param>
        /// <returns>The result value.</returns>
        public ExChangeRate GetExChangeRate(string currenyCode, DateTime datetime)
        {
            const string UspName = "Lrf_GetExchangeRateByDate";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    db.AddInParameter("CurrencyCode", DbType.String, currenyCode);
                    db.AddInParameter("Date", DbType.DateTime, datetime);

                    using (var reader = db.ExecuteReader())
                    {
                        var item = new ExChangeRate();
                        if (reader.Read())
                        {
                            item.USD = reader.ToDecimal("USD");
                            item.EUR = reader.ToDecimal("EUR");
                            item.GBP = reader.ToDecimal("GBP");
                        }

                        return item;
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