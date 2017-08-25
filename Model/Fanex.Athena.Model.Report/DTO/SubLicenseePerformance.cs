namespace Fanex.Athena.Model.Report.DTO
{
    public class SubLicenseePerformance
    {
        public string SubLicensee { get; set; }

        public string Currency { get; set; }

        public string Product { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal SubLicenseeWinLoss { get; set; }

        public decimal SubLicenseeComm { get; set; }
    }
}