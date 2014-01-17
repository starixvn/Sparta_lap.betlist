// -----------------------------------------------------------------------
// <copyright file="CustomerWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
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

        public decimal MemberWl { get; set; }

        public decimal PlayerComm { get; set; }

        public decimal MemberTotalWl { get; set; }

        public decimal LicenseeWl { get; set; }

        public decimal BaComm { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal AmtDueToBa { get; set; }

        public decimal AmtDueToRacing { get; set; }
    }
}