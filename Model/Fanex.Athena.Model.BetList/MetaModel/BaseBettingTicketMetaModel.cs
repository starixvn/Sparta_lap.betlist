using System;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class BaseBettingTicketMetaModel
    {
        private string _baseCurrency = string.Empty;

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int CurrencyId { get; set; }

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

        public string AgentListXml { get; set; }

        public bool IsConvert { get; set; }
    }
}