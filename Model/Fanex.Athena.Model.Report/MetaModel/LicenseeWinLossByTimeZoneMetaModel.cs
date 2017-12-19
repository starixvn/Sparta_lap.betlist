namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class LicenseeWinLossByTimeZoneMetaModel
    {
        public string AgentListXml { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }

        public string BaseCurrency { get; set; }

        public int CurrencyId { get; set; }

        public bool IsIncludeDrawData { get; set; }

        public bool IsConvertAll { get; set; }

        public bool IsExcludeTestData { get; set; }

        public short TimeZone { get; set; }

        public byte ReportBy { get; set; }

        public byte GroupBy { get; set; }

        public bool IsValid => FromDate <= ToDate &&
                                !string.IsNullOrEmpty(ProductCodeList) &&
                                !string.IsNullOrEmpty(BaseCurrency);
    }
}