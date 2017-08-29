namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.DataDomain;
    using Core.MetaModel.Product;

    public class SubsidiaryPerformanceViewModel
    {
        public SubsidiaryPerformanceViewModel()
        {
            FromDate = DateTime.Today;
            ToDate = DateTime.Today;
        }

        public IDictionary<int, string> SubsidiaryList { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public IEnumerable<string> BaseCurrencyList => SystemConstants.BaseCurrencyList;

        public bool ConvertAll { get; set; }
    }
}