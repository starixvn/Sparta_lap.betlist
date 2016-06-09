// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// PLAYTECH Casino Meta Model.
    /// </summary>
    public class PlaytechCasinoMetaModel : BaseReportMetaModel
    {
        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownCurrency { get; set; }

        public byte ReportBy { get; set; }

        public bool IsExcludedTestData { get; set; }

        public int CustId { get; set; }

        public int Level { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}