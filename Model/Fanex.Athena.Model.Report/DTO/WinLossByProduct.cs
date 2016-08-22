// -----------------------------------------------------------------------
// <copyright file="WinLossByProduct.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    /// <summary>
    /// WinLossByProduct class.
    /// </summary>
    public class WinLossByProduct : ICustomerWinLoss
    {
        public string ProductCode { get; set; }

        public DateTime Date { get; set; }

        public int RowSpan { get; set; }

        public int Index { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public string ProductName { get; set; }

        public decimal TurnOver { get; set; }

        public decimal TotalTransaction { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal LicenseeWLComm { get; set; }

        public decimal AmountDue { get; set; }

        #region ICustomerWinLoss Members

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }

        #endregion ICustomerWinLoss Members
    }
}