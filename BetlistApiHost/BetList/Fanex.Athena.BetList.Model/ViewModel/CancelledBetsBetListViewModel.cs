// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class CancelledBetsBetListViewModel : BetListBaseViewModel
    {
        public IEnumerable<CustomerCancelBetItem> CustomerList { get; set; }
    }
}