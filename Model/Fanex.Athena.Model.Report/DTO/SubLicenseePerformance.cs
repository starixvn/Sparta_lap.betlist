namespace Fanex.Athena.Model.Report.DTO
{
    public class SubLicenseePerformance
    {
        public string UserName { get; set; }

        public int CustId { get; set; }

        public string CurrencyName { get; set; }

        public decimal Turnover { get; set; }

        public decimal WinLost { get; set; }

        public decimal PlayerComm { get; set; }

        public int BetCount { get; set; }
    }
}