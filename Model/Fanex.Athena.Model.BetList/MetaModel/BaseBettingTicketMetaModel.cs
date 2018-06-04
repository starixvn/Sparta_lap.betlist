using System;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class BaseBettingTicketMetaModel
    {
        private string _baseCurrency = string.Empty;

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

        public bool IsConvert { get; set; }

        public virtual bool IsValid => true;
    }
}