// -----------------------------------------------------------------------
// <copyright file="LiveReportViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report.LiveReports
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// LiveReportViewModel class.
    /// </summary>
    [Serializable]
    public class LiveReportViewModel : BaseViewModel
    {
        public LiveReportMetalModel Param { get; set; }

        public IList<LiveReport> MainResult { get; set; }

        public bool IsAllowedNumberGame { get; set; }

        public DateTime BalanceUpDate { get; set; }
    }
}