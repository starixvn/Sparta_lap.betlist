// -----------------------------------------------------------------------
// <copyright file="LiveReportDetailViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel.LiveReports
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// LiveReportDetailViewModel class.
    /// </summary>
    public class LiveReportDetailViewModel : BaseViewModel
    {
        public IEnumerable<LiveReport> BySports { get; set; }

        public IEnumerable<LiveReport> ByCurrencies { get; set; }

        public IEnumerable<LiveReportByBetType> ByBetType { get; set; }

        public bool IsShowedDetailLink { get; set; }

        public LiveReportMetaModel Param { get; set; }

        public IList<Product> Products { get; set; }
    }
}