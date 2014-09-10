// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// AgCasinoWinLossMetaModel class.
    /// </summary>
    public class AgCasinoWinLossMetaModel : BaseReportMetaModel
    {
        public bool IsExcludedTestData { get; set; }

        public int DrillDownCurrencyId { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownLevel { get; set; }

        public int DrillDownCustomerId { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}