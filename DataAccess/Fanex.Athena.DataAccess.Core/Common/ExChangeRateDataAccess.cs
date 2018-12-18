// -----------------------------------------------------------------------
// <copyright file="ExChangeRateDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System;
    using System.Data;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common.Extensions;
    using Session.Interfaces;

    public class ExChangeRateDataAccess : BaseDataAccess, IExChangeRateDataAccess
    {
        public ExChangeRateDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting, sessionStorage)
        {
        }

        public ExChangeRate GetExChangeRate(string currenyCode, DateTime datetime)
        {
            const string UspName = "Lrf_GetExchangeRateByDate";
            using (var db = this.GetObjectDb(UspName))
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
    }
}