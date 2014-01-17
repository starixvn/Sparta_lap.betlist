// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Customer;

    /// <summary>
    /// CancelledBetsBetListViewModel class.
    /// </summary>
    public class CancelledBetsBetListViewModel : BetListBaseViewModel
    {
        public IList<CustomerCancelBetItem> CustomerList { get; set; }
    }
}