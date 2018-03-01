namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class ExchangeBetListSummaryMetaModel
    {
        public int CustId { get; set; }

        public int CurrencyId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string AgentListXml { get; set; }

        public string BaseCurrency { get; set; }
    }
}