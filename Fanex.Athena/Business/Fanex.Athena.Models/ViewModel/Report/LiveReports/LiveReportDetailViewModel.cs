// -----------------------------------------------------------------------
// <copyright file="LiveReportDetailViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report.LiveReports
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Report.LiveReports;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// LiveReportDetailViewModel class.
    /// </summary>
    public class LiveReportDetailViewModel : BaseViewModel
    {
        public IList<Fanex.Athena.Models.DTO.Report.LiveReports.LiveReport> BySports { get; set; }

        public IList<Fanex.Athena.Models.DTO.Report.LiveReports.LiveReport> ByCurrencies { get; set; }

        public IList<LiveReportByBetType> ByBetType { get; set; }

        public bool IsShowedDetailLink { get; set; }

        public LiveReportMetalModel Param { get; set; }

        public IList<Product> Products { get; set; }
    }
}