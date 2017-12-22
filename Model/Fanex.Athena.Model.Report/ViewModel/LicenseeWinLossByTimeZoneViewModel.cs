namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.MetaModel.Product;

    public class LicenseeWinLossByTimeZoneViewModel
    {
        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}