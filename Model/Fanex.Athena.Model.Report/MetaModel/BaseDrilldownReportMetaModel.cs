using System;

namespace Fanex.Athena.Model.Report.MetaModel
{
    public class BaseDrilldownReportMetaModel : BaseReportMetaModel
    {
        public int DrillDownCurrencyId { get; set; }

        public int DrillDownLevel { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownCustomerId { get; set; }
    }
}