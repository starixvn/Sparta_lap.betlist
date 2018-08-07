// -----------------------------------------------------------------------
// <copyright file="BalanceDetails.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    /// <summary>
    /// BalanceDetails class.
    /// </summary>
    public class BalanceDetails
    {
        public decimal LicWinLoss { get; set; }

        public decimal BetTradeCommission { get; set; }

        public decimal BACommission { get; set; }

        public decimal AmtDueToBetTrade { get; set; }

        public decimal AmtDueToBA { get; set; }

        public decimal CustomerCommission { get; set; }

        public string BalanceDateString { get; set; }
    }
}