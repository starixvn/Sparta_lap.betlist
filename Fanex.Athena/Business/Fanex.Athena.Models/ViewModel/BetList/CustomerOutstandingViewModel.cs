// -----------------------------------------------------------------------
// <copyright file="CustomerOutstandingViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetList;

    /// <summary>
    /// CustomerOutstandingViewModel class.
    /// </summary>
    public class CustomerOutstandingViewModel : BetListBaseViewModel
    {
        public IList<DailyStatement> DataList { get; set; }
    }
}