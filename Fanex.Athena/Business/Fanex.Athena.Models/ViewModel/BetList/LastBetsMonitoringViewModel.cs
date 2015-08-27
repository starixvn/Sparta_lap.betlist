// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using System.Collections.Generic;

    /// <summary>
    /// LastBetsMonitoringViewModel class.
    /// </summary>
    public class LastBetsMonitoringViewModel : BetListBaseViewModel
    {
        public IDictionary<string, int> SportsList { get; set; }

        public bool IsOnlyShowAllCurrencies { get; set; }
    }
}