using System.Collections.Generic;

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class NewParlayDetailMetaModel
    {
        public long TransId { get; set; }

        public int CustId { get; set; }

        public IDictionary<int, string> UserCurrencyList { get; set; }

        public string BetTeam { get; set; }

        public string BetCheck { get; set; }

        public decimal Handicap1 { get; set; }

        public decimal Handicap2 { get; set; }
    }
}