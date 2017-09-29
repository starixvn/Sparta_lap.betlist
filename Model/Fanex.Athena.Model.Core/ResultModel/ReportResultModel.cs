namespace Fanex.Athena.Model.Core.ResultModel
{
    using System.Collections.Generic;

    public class ReportResultModel<T>
    {
        public virtual IEnumerable<T> DataList { get; set; }

        public T TotalData { get; set; }

        public int TotalRows { get; set; }
    }
}