// -----------------------------------------------------------------------
// <copyright file="WinLossByProduct.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System;
    using Model.Core.Helper;

    /// <summary>
    /// WinLossByProduct class.
    /// </summary>
    public class WinLossByProduct : ICustomerWinLoss
    {
        private string _productCode;
        private string _productName;

        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        public DateTime Date { get; set; }

        public int RowSpan { get; set; }

        public int Index { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public string CurrencyName { get; set; }

        public int CurrencyId { get; set; }

        public string ProductName
        {
            get
            {
                if (string.IsNullOrEmpty(_productName))
                {
                    _productName = ProductHelper.GetProductName(_productCode);
                }

                return _productName;
            }
        }

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