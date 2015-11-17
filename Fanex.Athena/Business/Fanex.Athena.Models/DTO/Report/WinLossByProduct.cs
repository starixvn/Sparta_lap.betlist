// -----------------------------------------------------------------------
// <copyright file="WinLossByProduct.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System;
    using Fanex.Athena.Models.ViewModel;

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
                    _productName = GetProductName(_productCode);
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

        private static string GetProductName(string productCode)
        {
            switch (productCode.ToUpper())
            {
                case "BA":
                    return ViewResources.BA;

                case "LC":
                    return ViewResources.LiveCasino;

                case "NG":
                    return ViewResources.NumberGame;

                case "RB":
                    return ViewResources.Racing;

                case "SB":
                    return ViewResources.Sportsbook;

                case "VS":
                    return ViewResources.VirtualSports;

                case "VS180":
                    return ViewResources.VirtualSoccer;

                case "VS181":
                    return ViewResources.VirtualHorseRacing;

                case "VS182":
                    return ViewResources.VirtualGreyhounds;

                case "VS184":
                    return ViewResources.VirtualMotoRacing;

                case "VS186":
                    return ViewResources.VirtualTennis;

                case "CS":
                    return ViewResources.RNGCasino;

                case "BG":
                    return ViewResources.Bingo;

                case "KN":
                    return ViewResources.Keno;

                case "VS183":
                    return ViewResources.VirtualSpeedway;

                case "VS185":
                    return ViewResources.VirtualCycling;

                default:
                    return productCode;
            }
        }
    }
}