namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryPerformanceEntity : SubsidiaryPerformanceModelBase
    {
        public int SubsidiaryId { get; set; }

        public string Subsidiary { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }
    }
}
