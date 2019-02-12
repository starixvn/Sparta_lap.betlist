using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;

namespace Fanex.Athena.BetList.Model.ViewModel
{
    public class BetListResultViewModel
    {
        public List<TicketText> Main { get; set; }

        public BetListTotalData Total { get; set; }

        public string LeagueName { get; set; }

        public bool IsShowTotal { get; set; }
    }
}