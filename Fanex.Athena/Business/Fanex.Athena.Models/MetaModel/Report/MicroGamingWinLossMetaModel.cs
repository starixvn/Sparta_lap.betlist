// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// MicroGamingWinLossMetaModel class.
    /// </summary>
    public class MicroGamingWinLossMetaModel : BaseReportMetaModel
    {
        private DateTime? _fromDate;

        private DateTime? _toDate;

        public bool IsExcludedTestData { get; set; }

        public byte ReportBy { get; set; }

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

        public int ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public bool IsAllowReportby { get; set; }
    }
}