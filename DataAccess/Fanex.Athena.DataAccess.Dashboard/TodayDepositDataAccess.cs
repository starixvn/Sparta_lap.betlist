namespace Fanex.Athena.DataAccess.Dashboard
{
    using Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Athena.Model.Dashboard.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    public class TodayDepositDataAccess : BaseDataAccess, ISimpleWidgetDataAccess
    {
        public decimal GetData(DashboardBaseModel metaModel)
        {
            const string UspName = "Lrf_GetLicenseeTodayDeposit";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var param = new
                    {
                        LicenseeID = metaModel.LicenseeId,
                        metaModel.CurrencyIDList,
                        ReportDate = metaModel.FromDate
                    };

                    using (var reader = db.ExecuteReader(param))
                    {
                        return reader.Read() ? reader.ToDecimal("Deposit" + metaModel.BaseCurrency) : 0;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName, metaModel);
            }
        }
    }
}