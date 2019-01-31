using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;

namespace Fanex.Athena.BetList.Model.ViewModel
{
    public class ExchangeBetListViewModel
    {
        public IEnumerable<ExchangeBetList> MainData { get; set; }

        public decimal GrandTotal { get; set; }
    }
}
