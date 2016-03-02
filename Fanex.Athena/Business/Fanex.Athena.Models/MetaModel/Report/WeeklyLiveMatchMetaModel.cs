// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;

    /// <summary>
    /// RacingCommissionMetaModel class.
    /// </summary>
    public class WeeklyLiveMatchMetaModel : BaseReportMetaModel
    {
        public WeeklyLiveMatchMetaModel()
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