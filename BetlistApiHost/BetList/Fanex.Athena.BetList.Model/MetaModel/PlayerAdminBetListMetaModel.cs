namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System.Collections.Generic;

    public class PlayerAdminBetListMetaModel
    {
        public int CustId { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public string AgentListXml { get; set; }

        public decimal SumStake { get; set; }

        public string Currency { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}
