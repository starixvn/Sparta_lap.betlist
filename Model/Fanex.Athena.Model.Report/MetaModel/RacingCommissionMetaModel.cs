// -----------------------------------------------------------------------
// <copyright file="RacingCommissionMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// RacingCommissionMetaModel class.
    /// </summary>
    public class RacingCommissionMetaModel : BaseReportMetaModel
    {
        public RacingCommissionMetaModel()
        {
            FromDate = DateTime.Now;
            ToDate = DateTime.Now;
            PageSize = 1000;
            PageIndex = 1;
            SortColumn = "ExternalID";
            SortDirection = "DESC";
        }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public string SortDirection { get; set; }

        public string SortColumn { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}