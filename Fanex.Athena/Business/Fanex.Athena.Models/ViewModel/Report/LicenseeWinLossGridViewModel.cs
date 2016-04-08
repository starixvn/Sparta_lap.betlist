// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossGridViewModel.cs" company="Nexcel">
// LicenseeWinLossGridViewModel copyright.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System;

    /// <summary>
    /// LicenseeWinLossGridViewModel class.
    /// </summary>
    public class LicenseeWinLossGridViewModel
    {
        public int Index { get; set; }

        public DateTime Date { get; set; }

        public int RowSpan { get; set; }

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

        public decimal TotalBuyBackAmount { get; set; }

        public decimal TotalBuyBackAmountSingle
        {
            get
            {
                return TotalBuyBackAmount;
            }
        }

        public decimal ActualStake { get; set; }

        #region ICustomerWinLoss Members

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get
            {
                return CustomerWinLoss + CustomerComm;
            }
        }

        #endregion ICustomerWinLoss Members

        #region ILicenseeWL Members

        public decimal LicenseeWLSB { get; set; }

        public decimal LicenseeWLRB { get; set; }

        public decimal LicenseeWLNG { get; set; }

        public decimal LicenseeWLBA { get; set; }

        public decimal LicenseeWLLC { get; set; }

        public decimal LicenseeWLVS { get; set; }

        public decimal LicenseeWLBG { get; set; }

        public decimal LicenseeWLCS { get; set; }

        public decimal LicenseeWLKN { get; set; }

        public decimal LicenseeWLBT { get; set; }

        public decimal LicenseeWLAGC { get; set; }

        public decimal LicenseeWLMGCLC { get; set; }

        public decimal LicenseeWLMGCRNG { get; set; }

        public decimal LicenseeWLVB { get; set; }

        public decimal TotalLicenseeWL
        {
            get
            {
                return LicenseeWLBA + LicenseeWLLC + LicenseeWLNG + LicenseeWLRB + LicenseeWLSB + LicenseeWLVS + LicenseeWLBG + LicenseeWLCS + 
                    LicenseeWLKN + LicenseeWLBT + LicenseeWLAGC + LicenseeWLMGCLC + LicenseeWLMGCRNG;
            }
        }

        public decimal TotalLicenseeWLSingle
        {
            get { return TotalLicenseeWL; }
        }

        #endregion ILicenseeWL Members

        #region ITurnOver Members

        public decimal TurnOverSB { get; set; }

        public decimal TurnOverRB { get; set; }

        public decimal TurnOverNG { get; set; }

        public decimal TurnOverBA { get; set; }

        public decimal TurnOverLC { get; set; }

        public decimal TurnOverVS { get; set; }

        public decimal TurnOverBG { get; set; }

        public decimal TurnOverCS { get; set; }

        public decimal TurnOverKN { get; set; }

        public decimal TurnOverBT { get; set; }

        public decimal TurnOverAGC { get; set; }

        public decimal TurnOverMGCLC { get; set; }

        public decimal TurnOverMGCRNG { get; set; }

        public decimal TurnOverVB { get; set; }

        public decimal TotalTurnOver
        {
            get
            {
                return TurnOverBA + TurnOverLC + TurnOverNG + TurnOverRB + TurnOverSB + TurnOverVS + TurnOverBG + 
                    TurnOverCS + TurnOverKN + TurnOverBT + TurnOverAGC + TurnOverMGCLC + TurnOverMGCRNG;
            }
        }

        public decimal TotalTurnOverSingle
        {
            get { return TotalTurnOver; }
        }

        #endregion ITurnOver Members
    }
}