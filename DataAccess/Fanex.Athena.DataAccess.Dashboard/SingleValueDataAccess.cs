namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections;
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Model.Dashboard.MetaModel;

    public class SingleValueDataAccess : BaseDataAccess, ISingleValueDataAccess
    {
        public Task<decimal> GetCustomerBalance(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetCustomerBalance", "ClosingBalance");
        }

        public Task<decimal> GetPendingFunds(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseePendingFunds", "PendingFund", false);
        }

        public Task<decimal> GetTodayDeposit(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeTodayDeposit", "Deposit");
        }

        public Task<decimal> GetTodayWithdrawal(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeTodayWithdrawal", "Withdrawal");
        }

        private async Task<decimal> GetData(DashboardBaseModel metaModel, string uspName, string columnName, bool hasReportDate = true)
        {
            using (var db = this.GetObjectDb(uspName))
            {
                var param = new Hashtable();
                param.Add("LicenseeID", metaModel.LicenseeId);
                param.Add("CurrencyIDList", metaModel.CurrencyIDList);
                if (hasReportDate)
                {
                    param.Add("ReportDate", metaModel.FromDate);
                }

                using (var reader = await db.ExecuteReaderAsync(param).ConfigureAwait(false))
                {
                    return reader.Read() ? reader.ToDecimal(columnName + metaModel.BaseCurrency) : 0;
                }
            }
        }
    }
}