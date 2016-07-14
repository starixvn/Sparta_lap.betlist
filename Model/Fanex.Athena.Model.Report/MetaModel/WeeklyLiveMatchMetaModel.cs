// -----------------------------------------------------------------------
// <copyright file="WeeklyLiveMatchMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using Extension.Helper;

    public class WeeklyLiveMatchMetaModel : BaseReportMetaModel
    {
        public WeeklyLiveMatchMetaModel()
        {
            ToDate = DateTime.Now.AddDays(-1);
            FromDate = DateTimeHelper.GetFirstDateOfWeek(ToDate);
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