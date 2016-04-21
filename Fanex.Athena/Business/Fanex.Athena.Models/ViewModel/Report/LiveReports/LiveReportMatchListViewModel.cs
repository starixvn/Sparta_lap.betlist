// -----------------------------------------------------------------------
// <copyright file="LiveReportMatchListViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report.LiveReports
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// LiveReportMatchListViewModel class.
    /// </summary>
    public class LiveReportMatchListViewModel : BaseViewModel
    {
        public IList<LiveReportWinLossByMatch> MainResult { get; set; }

        public LiveReportMatchListMetalModel Param { get; set; }

        public string CurrencyName { get; set; }
    }
}