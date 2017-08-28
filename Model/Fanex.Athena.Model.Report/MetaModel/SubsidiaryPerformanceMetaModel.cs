namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.ComponentModel;

    public class SubsidiaryPerformanceMetaModel
    {
        private string _baseCurrency;

        public string AgentListXml { get; set; }

        public int LicenseeId { get; set; }

        public bool IsExportExcel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string SubsidiaryList { get; set; } = string.Empty;

        public string ProductCodeList { get; set; } = string.Empty;

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
                    : value.ToLower();
            }
        }

        public bool ConvertAll { get; set; }
    }
}