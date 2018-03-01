

using System;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class ExchangeBetListMetaModel
    {
        private string _baseCurrency = string.Empty;

        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public string AgentListXml { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool ExcludeTestData { get; set; }

        public int BetId { get; set; }

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
                    : value.ToUpperInvariant();
            }
        }
    }
}
