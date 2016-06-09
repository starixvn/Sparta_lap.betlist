// -----------------------------------------------------------------------
// <copyright file="LiveReportWinLossByMatch.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO.LiveReports
{
    /// <summary>
    /// LiveReportWinLossByMatch class.
    /// </summary>
    public class LiveReportWinLossByMatch : ICustomerWinLoss
    {
        public long MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int BetType { get; set; }

        public string ShowTime { get; set; }

        public int SportId { get; set; }

        public string MatchName { get; set; }

        public decimal TurnOver { get; set; }

        public int BetCount { get; set; }

        public decimal LicenseeComm { get; set; }

        public decimal AmountDue { get; set; }

        public decimal CustomerWinLoss
        {
            get;

            set;
        }

        public decimal CustomerComm
        {
            get;

            set;
        }

        public decimal CustomerTotal
        {
            get { return CustomerComm + CustomerWinLoss; }
        }

        public decimal LicenseeWinLoss
        {
            get;

            set;
        }
    }
}