namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryPerformanceEntity
    {
        public int SubsidiaryId { get; set; }

        public string Subsidiary { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string Product { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal SubsidiaryWinLoss { get; set; }
    }
}
