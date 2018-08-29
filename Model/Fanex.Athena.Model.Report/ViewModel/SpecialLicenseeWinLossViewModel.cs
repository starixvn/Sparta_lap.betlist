namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Core.DTO.Configuration;
    using MetaModel;

    public class SpecialLicenseeWinLossViewModel
    {
        public IDictionary<int, string> BaseCurrencyList { get; set; }

        public ReportSetting ReportSetting { get; set; }

        public LicenseeWinLossMetaModel Param { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}