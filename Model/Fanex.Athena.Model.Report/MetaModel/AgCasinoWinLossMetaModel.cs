// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// AgCasinoWinLossMetaModel class.
    /// </summary>
    public class AgCasinoWinLossMetaModel : BaseReportMetaModel
    {
        private DateTime? _fromDate;

        private DateTime? _toDate;

        public bool IsExcludedTestData { get; set; }

        public byte ReportBy { get; set; }

        public int DrillDownCurrencyId { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownLevel { get; set; }

        public int DrillDownCustomerId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public new DateTime FromDate
        {
            get { return _fromDate ?? new DateTime(Year, Month, FromDay); }
            set { _fromDate = value; }
        }

        public new DateTime ToDate
        {
            get { return _toDate ?? new DateTime(Year, Month, ToDay); }
            set { _toDate = value; }
        }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}