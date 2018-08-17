namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Core.DTO.Configuration;
    using Core.ViewModel;
    using MetaModel;

    public class SpecialLicenseeWinLossViewModel : BaseViewModel
    {
        public IDictionary<int, string> BaseCurrencyList { get; set; }

        public ReportSetting ReportSetting { get; set; }

        public LicenseeWinLossMetaModel Param { get; set; }
    }
}