using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Model.Report.DTO;

namespace Fanex.Athena.Model.Report.ResultModel
{
    public class ActiveCustomerSummaryResultModel
    {
        public SortableList<ActiveCustomer> DataList { get; set; }

        public int TotalCustomer { get; set; }
    }
}