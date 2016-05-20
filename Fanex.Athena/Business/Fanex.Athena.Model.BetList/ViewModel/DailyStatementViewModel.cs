// -----------------------------------------------------------------------
// <copyright file="DailyStatementViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetList.DTO;

    /// <summary>
    /// DailyStatementViewModel class.
    /// </summary>
    public class DailyStatementViewModel : BetListBaseViewModel
    {
        public IList<DailyStatement> DataList { get; set; }
    }
}