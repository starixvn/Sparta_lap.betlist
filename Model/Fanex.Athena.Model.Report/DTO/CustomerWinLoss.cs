// -----------------------------------------------------------------------
// <copyright file="CustomerWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    /// <summary>
    /// CustomerWinLoss class.
    /// </summary>
    public class CustomerWinLoss
    {
        public int CustId { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public decimal Turnover { get; set; }

        public int TotalTransaction { get; set; }

        public decimal MemberWL { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal MemberTotalWL { get; set; }

        public decimal BetEffective { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal BAComm { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }
    }
}