using System.Collections.Generic;

namespace Fanex.Athena.Model.Report.DTO
{
    public class SubLicenseePerformance
    {
        public string SubLicensee { get; set; }

        public string Currency { get; set; }

        public IEnumerable<SubLicenseePerformanceDetail> Details { get; set; }
    }
}