namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class CashOutBetListDetailViewModel
    {
        public IEnumerable<TicketText> TicketTextList { get; set; }

        [System.CLSCompliant(false)]
        public IEnumerable<LrfTicket> DataList { get; set; }

        public IEnumerable<CashOutTicket> CashOutTicketList { get; set; }

        public IEnumerable<CashOutTicketText> CashOutTicketTextList { get; set; }
    }
}