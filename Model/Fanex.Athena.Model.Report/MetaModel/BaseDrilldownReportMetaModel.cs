using System;
using Fanex.Athena.Extension.Utility;

namespace Fanex.Athena.Model.Report.MetaModel
{
    public class BaseDrilldownReportMetaModel : BaseReportMetaModel
    {
        public BaseDrilldownReportMetaModel() : this(null)
        {
        }

        public BaseDrilldownReportMetaModel(IAppSettings appSetting) : base(appSetting ?? new AppSettings())
        {
        }

        public int DrillDownCurrencyId { get; set; }

        public int DrillDownLevel { get; set; }

        public DateTime DrillDownFromDate { get; set; }

        public DateTime DrillDownToDate { get; set; }

        public int DrillDownCustomerId { get; set; }
    }
}