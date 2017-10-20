namespace Fanex.Athena.Model.Core.ResultModel
{
    using System.Collections.Generic;
    using Extension.Utility;

    public class SortableSummaryReportResultModel<T>
    {
        public virtual SortableList<T> DataList { get; set; }

        public IEnumerable<T> TotalData { get; set; }

        public IDictionary<string, T> SummaryTotal { get; set; }

        public int TotalRows { get; set; }
    }
}