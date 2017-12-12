namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.DataDomain;

    public class ExchangeWinLossViewModel
    {
        public ExchangeWinLossViewModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
            ReportBy = ReportByType.Daily;
            ExcludeTestData = true;
            CurrencyList = new Dictionary<int, string>();
            ReportByOption = new Dictionary<byte, string>();
            ExcludeTestDataOption = new Dictionary<bool, string>();
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte ReportBy { get; set; }

        public bool ExcludeTestData { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public int CurrencyId { get; set; }

        public int LicenseeId { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }

        public IDictionary<byte, string> ReportByOption { get; set; }

        public IDictionary<bool, string> ExcludeTestDataOption { get; set; }
    }
}
