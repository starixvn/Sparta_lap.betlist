// -----------------------------------------------------------------------
// <copyright file="LiveReportMatchListViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel.LiveReports
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
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