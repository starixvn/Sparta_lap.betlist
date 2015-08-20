// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// LicenseeWinLossMetaModel class.
    /// </summary>
    public class LicenseeWinLossMetaModel : BaseReportMetaModel
    {
        public byte ReportBy { get; set; }

        public int DrawFilter { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int DrillDownCurrencyId { get; set; }

        public string DrillDownCurrencyName { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownLevel { get; set; }

        public int DrillDownCustomerId { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}