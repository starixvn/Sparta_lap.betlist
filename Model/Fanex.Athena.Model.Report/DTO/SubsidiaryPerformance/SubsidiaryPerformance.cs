namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryPerformance : SubsidiaryPerformanceBase
    {
        public int SubsidiaryId { get; set; }

        public int CurrencyId { get; set; }

        public string ProductCode { get; set; }

        public string Subsidiary { get; set; }

        public SubsidiaryPerformanceBase BaseCurrencyData { get; set; }
    }
}
