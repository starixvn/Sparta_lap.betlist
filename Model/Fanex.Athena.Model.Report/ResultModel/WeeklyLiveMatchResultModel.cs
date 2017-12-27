using System.Collections.Generic;
using Fanex.Athena.Model.Report.DTO;

namespace Fanex.Athena.Model.Report.ResultModel
{
    public class WeeklyLiveMatchResultModel
    {
        public IEnumerable<WeeklyLiveMatch> DataList { get; set; }

        public int[] TotalData { get; set; }
    }
}