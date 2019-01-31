using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.BetList.Model.DTO
{
    public class BetListTotalData
    {
        public decimal SumWinLoss { get; set; }
        public decimal SumLicenseeWl { get; set; }
        public decimal SumBetEffective { get; set; }
        public decimal SumStake { get; set; }
    }
}
