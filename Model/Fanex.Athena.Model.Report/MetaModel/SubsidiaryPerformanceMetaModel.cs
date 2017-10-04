namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    public class SubsidiaryPerformanceMetaModel
    {
        public string AgentListXml { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }

        public string SubsidiaryList { get; set; } = string.Empty;

        public string BaseCurrency { get; set; }

        public bool ConvertAll { get; set; }

        public bool IsValid => FromDate <= ToDate &&
                                !string.IsNullOrEmpty(ProductCodeList) &&
                                !string.IsNullOrEmpty(BaseCurrency) &&
                                !string.IsNullOrEmpty(SubsidiaryList);
    }
}