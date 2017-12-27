using System.Collections.Generic;

namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryResultViewModel
    {
        public IEnumerable<SubsidiaryPerformanceDataModel> Data { get; set; }

        public IEnumerable<SubsidiaryPerformanceEntity> Total { get; set; }

        public SubsidiaryPerformanceEntity GrandTotal { get; set; }
    }
}
