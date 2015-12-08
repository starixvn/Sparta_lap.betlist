// -----------------------------------------------------------------------
// <copyright file="WinLossByLeagueViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// TurnoverViewModel class.
    /// </summary>
    public class WinLossByLeagueViewModel : BaseViewModel
    {
        public IEnumerable<WinLossByLeague> MainResult { get; set; }

        public WinLossByLeagueMetaModel Param { get; set; }

        public IDictionary<int, string> Sports { get; set; }

        public IDictionary<int, string> Leagues { get; set; }

        public IDictionary<int, string> Matches { get; set; }
    }
}