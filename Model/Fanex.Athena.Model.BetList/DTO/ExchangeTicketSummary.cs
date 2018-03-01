namespace Fanex.Athena.Model.BetList.DTO
{
    using System.Collections.Generic;

    public class ExchangeTicketSummary
    {
        public ExchangeBetList Ticket { get; set; }

        public IEnumerable<ExchangeBetListEntity> TicketList { get; set; }
    }
}