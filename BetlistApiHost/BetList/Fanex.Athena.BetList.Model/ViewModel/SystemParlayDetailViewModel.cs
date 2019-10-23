using System;
using System.Collections.Generic;
using System.Linq;
using Fanex.Athena.BetList.Model.DTO;

namespace Fanex.Athena.BetList.Model.ViewModel
{
    public class SystemParlayDetailViewModel
    {
        public IEnumerable<IGrouping<DateTime, SystemParlayDetailItem>> DataList { get; set; }

        public decimal Outstanding { get; set; }

        public decimal Total { get; set; }

        public bool ShowPlayerComm { get; set; }
    }
}