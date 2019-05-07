namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class LicenseeWinLossByTimeZone
    {
        private const int OneHundredPercent = 100;

        public DateTime ReportBy { get; set; }

        public string GroupBy { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal LicenseeWinLoss { get; set; }

        public decimal LicenseeComm { get; set; }

        public decimal LicenseeNetWinLoss
        {
            get
            {
                return LicenseeWinLoss - CustomerComm + LicenseeComm;
            }
        }

        public decimal Margin
        {
            get
            {
                return Turnover == 0 ? 0 : (LicenseeNetWinLoss / Turnover) * OneHundredPercent;
            }
        }
    }
}