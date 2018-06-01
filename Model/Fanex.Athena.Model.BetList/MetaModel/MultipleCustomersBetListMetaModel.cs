namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class MultipleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public DateTime Date { get; set; }

        public string ProductCode { get; set; }

        public int LiveStatus { get; set; }

        public string Status { get; set; }
    }
}