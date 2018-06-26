namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class BaseBettingTicketMetaModel
    {
        private string _baseCurrency = string.Empty;

        public BaseBettingTicketMetaModel()
        {
            PageIndex = 1;
            PageSize = 50;
        }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

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

        public bool IsConvertAll { get; set; }

        public string AgentListXml { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCodeList { get; set; }

        public int CustId { get; set; }

        public bool IsExport { get; set; }

        public virtual bool IsValid => FromDate <= ToDate && !string.IsNullOrWhiteSpace(ProductCodeList);
    }
}