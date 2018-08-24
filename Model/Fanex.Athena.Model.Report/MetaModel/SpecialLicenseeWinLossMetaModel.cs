namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.ComponentModel;
    using Core.MetaModel;

    public class SpecialLicenseeWinLossMetaModel
    {
        private string _baseCurrency = string.Empty;
        public int CurrencyId { get; set; }

        [DefaultValue("")]
        public string CurrencyName { get; set; }

        [DefaultValue("")]
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
                    : value.ToLowerInvariant();
            }
        }
        public byte ReportBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }
        public string[] ProductCodes => ProductCodeList?.Split(',');
        public string[] BaseCurrencies { get; set; }
        public string SiteName { get; set; }
        public string CurrencyListXml { get; set; }

    }
}