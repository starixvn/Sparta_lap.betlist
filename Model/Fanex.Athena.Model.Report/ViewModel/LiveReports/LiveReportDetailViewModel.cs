// -----------------------------------------------------------------------
// <copyright file="LiveReportDetailViewModel.cs" company="">
// warehouse winloss form parameters
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
        public IList<Fanex.Athena.Model.Report.DTO.LiveReports.LiveReport> BySports { get; set; }

        public IList<Fanex.Athena.Model.Report.DTO.LiveReports.LiveReport> ByCurrencies { get; set; }

        public IList<LiveReportByBetType> ByBetType { get; set; }

        public bool IsShowedDetailLink { get; set; }

        public LiveReportMetaModel Param { get; set; }

        public IList<Product> Products { get; set; }
    }
}