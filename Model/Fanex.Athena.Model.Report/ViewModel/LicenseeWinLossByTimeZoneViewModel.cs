namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.MetaModel.Product;

    public class LicenseeWinLossByTimeZoneViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Now.AddMonths(-1);

        public DateTime ToDate { get; set; } = DateTime.Today;

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public bool ConvertAll { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}