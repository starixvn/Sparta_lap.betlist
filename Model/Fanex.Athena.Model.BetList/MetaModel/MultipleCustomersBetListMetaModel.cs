namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class MultipleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public DateTime Date { get; set; }

        public string ProductCode { get; set; }

        public int LiveStatus { get; set; }

        public string Status { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public string AgentListXml { get; set; }
    }
}