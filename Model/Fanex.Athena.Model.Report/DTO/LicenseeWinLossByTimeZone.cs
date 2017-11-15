namespace Fanex.Athena.Model.Report.DTO
{
    public class LicenseeWinLossByTimeZone
    {
        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string Product { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal LicenseeWinLoss { get; set; }

        public decimal LicenseeNetWinLoss { get; set; }

        public decimal Margin { get; set; }
    }
}
