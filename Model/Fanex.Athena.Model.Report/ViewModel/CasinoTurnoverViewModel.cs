namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.MetaModel.Product;
    using Core.ViewModel;

    public class CasinoTurnoverViewModel : BaseViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }

        public IDictionary<int, string> Status { get; set; }

        public IEnumerable<string> BaseCurrencies { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; }
    }
}