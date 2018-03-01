using System.Collections.Generic;
using Fanex.Athena.Model.BetList.DTO;

namespace Fanex.Athena.Model.BetList.ViewModel
{
    public class ExchangeBetListSummaryViewModel
    {
        public IEnumerable<ExchangeTicketSummary> MainData { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
