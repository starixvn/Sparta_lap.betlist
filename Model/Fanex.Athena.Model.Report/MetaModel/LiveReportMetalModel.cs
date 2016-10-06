// -----------------------------------------------------------------------
// <copyright file="LiveReportMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;

    /// <summary>
    /// LiveReportMetalModel class.
    /// </summary>
    [Serializable]
    public class LiveReportMetalModel : BaseReportMetaModel
    {
        public bool IsViewedHistory { get; set; }

        public int LiveIndicator { get; set; }

        public DateTime MinDate
        {
            get
            {
                return DateTime.Today.AddDays(1 - DateTime.Today.Day).AddMonths(-3);
            }
        }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}