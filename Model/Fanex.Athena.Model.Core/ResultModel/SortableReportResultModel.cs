namespace Fanex.Athena.Model.Core.ResultModel
{
    using Extension.Utility;

    public class SortableReportResultModel<T> : ReportResultModel<T>
    {
        public new SortableList<T> DataList { get; set; }
    }
}