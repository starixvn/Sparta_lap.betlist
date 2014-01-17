// -----------------------------------------------------------------------
// <copyright file="MatchWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// MatchWinLossViewModel class.
    /// </summary>
    public class MatchWinLossViewModel : BaseViewModel
    {
        public IDictionary<int, string> LeagueList { get; set; }

        public IList<MatchWinLoss> MainResult { get; set; }

        public IList<MatchWinLoss> SpecialBetTypeResult { get; set; }

        public MatchWinLossMetaModel Param { get; set; }

        public int ColumnCount { get; set; }

        public MatchWinLoss Total { get; set; }

        public MatchWinLoss GrandTotal { get; set; }
    }
}