namespace Fanex.Athena.DataAccess.Dashboard
{
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class TodayDepositDataAccess : ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseeTodayDeposit";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        UserID = metaModel.UserId,
                        CurrencyID = metaModel.CurrencyId,
                        Today = metaModel.FromDate
                    };

                    using (var reader = db.ExecuteReader(param))
                    {
                        return reader.Read() ? reader.ToDecimal("Deposit" + metaModel.BaseCurrency) : 0;
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