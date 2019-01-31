// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    /// <summary>
    /// LastBetsMonitoringViewModel class.
    /// </summary>
    public class LastBetsMonitoringViewModel : BetListBaseViewModel
    {
        public IEnumerable<SelectListItem> SportsList { get; set; }
    }
}