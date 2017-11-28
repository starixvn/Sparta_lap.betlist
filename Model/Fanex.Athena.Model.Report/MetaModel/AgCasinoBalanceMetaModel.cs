// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// AgCasinoWinLossMetaModel class.
    /// </summary>
    public class AgCasinoBalanceMetaModel : BaseDrilldownReportMetaModel
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        private DateTime? _fromDate;

        private DateTime? _toDate;

        public bool IsExcludedTestData { get; set; }

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
        
        public override bool IsValid
        {
            get
            {
                return true;
            }
        }
    }
}