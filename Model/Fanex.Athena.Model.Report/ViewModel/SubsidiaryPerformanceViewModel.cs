namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.MetaModel.Product;

    public class SubsidiaryPerformanceViewModel
    {
        private string _baseCurrency;

        public IDictionary<int, string> SubsidiaryList { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public string BaseCurrency
        {
            get
            {
                return _baseCurrency;
            }

            set
            {
                _baseCurrency = value == null
                    ? string.Empty
                    : value.ToLower();
            }
        }

        public bool ConvertAll { get; set; }
    }
}