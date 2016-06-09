// -----------------------------------------------------------------------
// <copyright file="CancelledBetsBetListViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    /// <summary>
    /// CancelledBetsBetListViewModel class.
    /// </summary>
    public class CancelledBetsBetListViewModel : BetListBaseViewModel
    {
        public IList<CustomerCancelBetItem> CustomerList { get; set; }
    }
}