// -----------------------------------------------------------------------
// <copyright file="DailyStatementViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.BetList;

    /// <summary>
    /// DailyStatementViewModel class.
    /// </summary>
    public class DailyStatementViewModel : BetListBaseViewModel
    {
        public IList<DailyStatement> DataList { get; set; }
    }
}