// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// LicenseeWinLoss class.
    /// </summary>
    [Serializable]
    public class LicenseeWinLoss : ICustomerWinLoss, ILicenseeWL, ITurnOver
    {
        public DateTime Date { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal BACommission { get; set; }

        public decimal BetTradeCommission { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }

        public decimal AmtDueToBetTrade { get; set; }

        public decimal BuyBackAmountSB { get; set; }

        public decimal BuyBackAmountBA { get; set; }

        public decimal TotalBuyBackAmount
        {
            get
            {
                return BuyBackAmountSB + BuyBackAmountBA;
            }
        }

        public decimal ActualStake { get; set; }

        #region ICustomerWinLoss Members

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }

        #endregion ICustomerWinLoss Members

        #region ILicenseeWL Members

        public IDictionary<string, decimal> LicenseeWL { get; set; }

        public decimal TotalLicenseeWL
        {
            get { return LicenseeWL.Sum(i => i.Value); }
        }

        #endregion ILicenseeWL Members

        #region ITurnOver Members

        public IDictionary<string, decimal> TurnOver { get; set; }

        public decimal TotalTurnOver
        {
            get { return TurnOver.Sum(i => i.Value); }
        }

        #endregion ITurnOver Members
    }
}