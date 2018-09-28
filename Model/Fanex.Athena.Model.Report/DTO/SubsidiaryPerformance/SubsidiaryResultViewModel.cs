using System.Collections.Generic;

namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryResultViewModel
    {
        public IEnumerable<SubsidiaryPerformanceDataModel> Data { get; set; }

        public IDictionary<string, SubsidiaryPerformanceBase> Total { get; set; }

        public SubsidiaryPerformanceBase GrandTotal { get; set; }
    }
}
