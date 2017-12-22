using System;

namespace Fanex.Athena.Model.Report.DTO
{
    public class LicenseeWinLossByTimeZone
    {
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
                return Turnover == 0 ? 0 : (LicenseeNetWinLoss / Turnover) * 100;
            }
        }
    }
}