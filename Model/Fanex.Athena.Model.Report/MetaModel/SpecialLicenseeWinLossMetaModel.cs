namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Core.MetaModel;

    public class SpecialLicenseeWinLossMetaModel : BaseMetaModel
    {
        public byte ReportBy { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public string ProductCodeList { get; set; }

        public string[] ProductCodes { get; set; }

        public string[] BaseCurrencies { get; set; }
    }
}