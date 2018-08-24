namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Core.MetaModel;

    public class SpecialLicenseeWinLossMetaModel : BaseMetaModel
    {
        public byte ReportBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }
        public string[] ProductCodes => ProductCodeList?.Split(',');
        public string[] BaseCurrencies { get; set; }
        public string SiteName { get; set; }

        public string CurrencyCodes { get; set; }

        public string CurrencyIdList { get; set; }
    }
}