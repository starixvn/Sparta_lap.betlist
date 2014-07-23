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

        public decimal RacingRevenue { get; set; }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }

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

        public decimal LicenseeWlsb { get; set; }

        public decimal LicenseeWlrb { get; set; }

        public decimal LicenseeWlng { get; set; }

        public decimal LicenseeWlba { get; set; }

        public decimal LicenseeWllc { get; set; }

        public decimal LicenseeWlvs { get; set; }

        public decimal LicenseeWlbg { get; set; }

        public decimal LicenseeWlcs { get; set; }

        public decimal LicenseeWlkn { get; set; }

        public decimal TotalLicenseeWL
        {
            get { return LicenseeWlba + LicenseeWllc + LicenseeWlng + LicenseeWlrb + LicenseeWlsb + LicenseeWlvs + LicenseeWlbg + LicenseeWlcs + LicenseeWlkn; }
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

        public decimal TotalTurnOver
        {
            get { return TurnOverBA + TurnOverLC + TurnOverNG + TurnOverRB + TurnOverSB + TurnOverVS + TurnOverBG + TurnOverCS + TurnOverKN; }
        }

        public decimal TotalTurnOverSingle
        {
            get { return TotalTurnOver; }
        }

        #endregion ITurnOver Members
    }
}