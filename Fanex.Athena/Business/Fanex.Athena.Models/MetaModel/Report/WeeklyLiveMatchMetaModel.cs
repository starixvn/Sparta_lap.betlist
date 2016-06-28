// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;
    using Extension.Helper;

    public class WeeklyLiveMatchMetaModel : BaseReportMetaModel
    {
        public WeeklyLiveMatchMetaModel()
        {
            FromDate = DateTimeHelper.GetFirstDateOfWeek(DateTime.Now);
            ToDate = DateTime.Now.AddDays(-1);
        }

        public string SortDirection { get; set; }

        public string SortColumn { get; set; }

        public int NumberOfWeeks
        {
            get
            {
                return DateTimeHelper.GetWeeks(FromDate, ToDate);
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