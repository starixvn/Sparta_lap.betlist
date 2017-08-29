namespace Fanex.Athena.Model.Report.DTO.SubsidiaryPerformance
{
    public class SubsidiaryPerformanceEntity
    {
        public string Subsidiary { get; set; }

        public string Currency { get; set; }

        public string Product { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal SubsidiaryWinLoss { get; set; }

        public decimal SubsidiaryComm { get; set; }
    }
}
