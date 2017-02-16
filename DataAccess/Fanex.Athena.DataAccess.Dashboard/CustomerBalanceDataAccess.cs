namespace Fanex.Athena.DataAccess.Dashboard
{
    using System;
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Logging;
    using Model.Dashboard.MetaModel;

    public class CustomerBalanceDataAccess : BaseDataAccess, ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetCustomerBalance";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        CurrencyID = metaModel.CurrencyId,
                        ReportDate = metaModel.FromDate
                    };

                    var result = db.ExecuteScalar(param);
                    return result == DBNull.Value ? 0 : (decimal)result;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, metaModel);
            }
        }
    }
}