// -----------------------------------------------------------------------
// <copyright file="CustomerOutstandingViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;

    /// <summary>
    /// CustomerOutstandingViewModel class.
    /// </summary>
    public class CustomerOutstandingViewModel : BetListBaseViewModel
    {
        public IList<DailyStatement> DataList { get; set; }
    }
}