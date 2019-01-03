namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections;
    using Core;
    using Core.Exception;
    using Data;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Fanex.Common.Extensions;
    using Model.Dashboard.MetaModel;

    public class SingleValueDataAccess : BaseDataAccess, ISingleValueDataAccess
    {
        public decimal GetCustomerBalance(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetCustomerBalance", "ClosingBalance");
        }

        public decimal GetPendingFunds(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseePendingFunds", "PendingFund", false);
        }

        public decimal GetTodayDeposit(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeTodayDeposit", "Deposit");
        }

        public decimal GetTodayWithdrawal(DashboardBaseModel metaModel)
        {
            return GetData(metaModel, "Lrf_GetLicenseeTodayWithdrawal", "Withdrawal");
        }

        private decimal GetData(DashboardBaseModel metaModel, string uspName, string columnName, bool hasReportDate = true)
        {
            try
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

                    using (var reader = db.ExecuteReader(param))
                    {
                        return reader.Read() ? reader.ToDecimal(columnName + metaModel.BaseCurrency) : 0;
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, uspName, metaModel);
            }
        }
    }
}