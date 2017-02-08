namespace Fanex.Athena.DataAccess.Dashboard
{
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Logging;
    using Model.Dashboard.MetaModel;

    public class PendingFundDataAccess : ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseePendingFunds";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        UserID = metaModel.UserId,
                        CurrencyID = metaModel.CurrencyId
                    };

                    using (var reader = db.ExecuteReader(param))
                    {
                        return reader.Read() ? reader.ToDecimal("PendingFund" + metaModel.BaseCurrency) : 0;
                    }
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