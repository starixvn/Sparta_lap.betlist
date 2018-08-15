namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Core.DTO.Configuration;
    using Core.MetaModel.Product;
    using Core.ViewModel;

    public class SpecialLicenseeWinLossViewModel : BaseViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int LicenseeId { get; set; }

        public int CurrencyId { get; set; }

        public IDictionary<int, string> BaseCurrencyList { get; set; }

        public IEnumerable<ProductOption> ProductOptionList { get; set; }

        public ReportSetting ReportSetting { get; set; }
    }
}