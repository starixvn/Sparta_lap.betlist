using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Model.BetList.DTO;

namespace Fanex.Athena.Model.BetList.ViewModel
{
    public class BetListResultViewModel
    {
        public List<TicketText> Main { get; set; }

        public BetListTotalData Total { get; set; }

        public string LeagueName { get; set; }

        public bool IsShowTotal { get; set; }
    }
}
