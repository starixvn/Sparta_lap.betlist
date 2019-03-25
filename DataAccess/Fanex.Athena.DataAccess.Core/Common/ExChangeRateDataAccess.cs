// -----------------------------------------------------------------------
// <copyright file="ExChangeRateDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Data;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;

    public class ExChangeRateDataAccess : BaseDataAccess, IExChangeRateDataAccess
    {
        public ExChangeRate GetExChangeRate(string currenyCode, DateTime datetime)
        {
            const string UspName = "Lrf_GetExchangeRateByDate";
            using (var db = this.GetObjectDb(UspName))
            {
                db.SetParameterValue("CurrencyCode", currenyCode);
                db.SetParameterValue("Date", datetime);

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
    }
}