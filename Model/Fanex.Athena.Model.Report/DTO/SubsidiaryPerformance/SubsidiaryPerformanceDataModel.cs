namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    using System.Collections.Generic;

    public class SubsidiaryPerformanceDataModel
    {
        public string Subsidiary { get; set; }

        public string Currency { get; set; }

        public IEnumerable<SubsidiaryPerformanceDetail> Details { get; set; }
    }
}