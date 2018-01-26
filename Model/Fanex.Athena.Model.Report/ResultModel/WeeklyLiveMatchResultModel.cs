namespace Fanex.Athena.Model.Report.ResultModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;

    public class WeeklyLiveMatchResultModel
    {
        public IEnumerable<WeeklyLiveMatch> DataList { get; set; }

        public int[] TotalData { get; set; }

        public int NumberOfWeeks { get; set; }
    }
}