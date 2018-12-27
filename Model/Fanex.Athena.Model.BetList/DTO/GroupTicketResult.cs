using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.BetList.DTO
{
    public class GroupTicketResult
    {
        public IList<string> DataPath { get; set; } = new List<string>();
        public int RowPerGroup { get; set; }
        public int TotalRow { get; set; }
        // Sum value
        public int TotalRows { get; set; }

        public decimal SumWinLoss { get; set; }

        public decimal SumBetEffective { get; set; }

        public decimal SumLicenseeWL { get; set; }

        public decimal SumComm { get; set; }

        public decimal SumPlayerComm { get; set; }

        public decimal SumStake { get; set; }
    }
}
