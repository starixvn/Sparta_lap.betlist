// -----------------------------------------------------------------------
// <copyright file="BalanceDetails.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    /// <summary>
    /// BalanceDetails class.
    /// </summary>
    public class BalanceDetails
    {
        public decimal LicWinLoss { get; set; }

        public decimal BACommission { get; set; }

        public decimal RBCommission { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal RacingWinLoss
        {
            get { return RBCommission + RacingRevenue; }
        }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }

        public decimal CustomerCommission { get; set; }

        public string BalanceDateString { get; set; }
    }
}