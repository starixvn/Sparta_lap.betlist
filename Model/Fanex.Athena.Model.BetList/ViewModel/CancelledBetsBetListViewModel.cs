namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class CancelledBetsBetListViewModel : BetListBaseViewModel
    {
        public IEnumerable<CustomerCancelBetItem> CustomerList { get; set; }
    }
}