using System.Collections.Generic;
using Fanex.Athena.Model.BetList.DTO;

namespace Fanex.Athena.Model.BetList.ViewModel
{
    public class ExchangeBetListViewModel
    {
        public IEnumerable<ExchangeBetList> MainData { get; set; }

        public decimal GrandTotal { get; set; }
    }
}