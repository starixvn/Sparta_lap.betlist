namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.Linq;

    public class CasinoTurnoverMetaModel
    {
        private string[] _productCode;

        private int[] _status;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int CurrencyId { get; set; }

        public string BaseCurrency { get; set; }

        public bool IsConvertAll { get; set; }

        public string[] ProductCode
        {
            get
            {
                return _productCode;
            }

            set
            {
                _productCode = value ?? new string[0];
            }
        }

        public string UserName { get; set; }

        public int[] Status
        {
            get
            {
                return _status;
            }

            set
            {
                _status = value ?? new int[0];
            }
        }

        public string AgentListXml { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public bool IsExportExcel { get; set; }

        public bool IsValid => FromDate <= ToDate && ProductCode.Any() && Status.Any();
    }
}