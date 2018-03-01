namespace Fanex.Athena.Model.BetList.DTO
{
    using System.Collections.Generic;

    public class ExchangeTicketSummary : ExchangeBetListBase
    {
        public IEnumerable<ExchangeBetListEntity> TicketList { get; set; }
    }
}