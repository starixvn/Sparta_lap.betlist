using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Model.BetList.DTO;

namespace Fanex.Athena.Excel.BetList.Model
{
    public class BettingTicketCenterExcelModel
    {
        public TicketResult Data { get; set; }

        public bool ShowTotal { get; set; }

        public bool ShowSumStake { get; set; }

        public bool ShowWinLoss { get; set; }

        public bool ShowLicenseeWl { get; set; }

        public bool CustomerCommVisible { get; set; }

        public int LicenseeId { get; set; }

        public string Title { get; set; }
    }
}
