namespace Fanex.Athena.DataAccess.Dashboard
{
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Logging;
    using Model.Dashboard.MetaModel;

    public class TodayWithdrawalDataAccess : BaseDataAccess, ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseeTodayWithdrawal";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        UserID = metaModel.UserId,
                        CurrencyID = metaModel.CurrencyId,
                        ReportDate = metaModel.FromDate
                    };

                    using (var reader = db.ExecuteReader(param))
                    {
                        return reader.Read() ? reader.ToDecimal("Withdrawal" + metaModel.BaseCurrency) : 0;
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