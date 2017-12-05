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
        }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public byte ReportBy { get; set; }

        public bool ExcludeTestData { get; set; }

        public int CurrencyId { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}
