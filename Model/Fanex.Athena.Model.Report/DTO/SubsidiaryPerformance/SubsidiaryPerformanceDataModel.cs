namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    using System.Collections.Generic;

    public class SubsidiaryPerformanceDataModel
    {
        public string Subsidiary { get; set; }

        public int TotalItem { get; set; }

        public IDictionary<string, IEnumerable<SubsidiaryPerformanceBase>> Details { get; set; }
    }
}