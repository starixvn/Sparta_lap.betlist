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
                    switch (_productCode.ToUpper())
                    {
                        case "BA":
                            _productName = ViewResources.BA;
                            break;
                        case "LC":
                            _productName = ViewResources.LiveCasino;
                            break;
                        case "NG":
                            _productName = ViewResources.NumberGame;
                            break;
                        case "RB":
                            _productName = ViewResources.Racing;
                            break;
                        case "SB":
                            _productName = ViewResources.Sportsbook;
                            break;
                        case "VS":
                            _productName = ViewResources.VirtualSports;
                            break;
                        case "VS180":
                            _productName = ViewResources.VirtualSoccer;
                            break;
                        case "VS181":
                            _productName = ViewResources.VirtualHorseRacing;
                            break;
                        case "VS182":
                            _productName = ViewResources.VirtualGreyhounds;
                            break;
                        case "VS184":
                            _productName = ViewResources.VirtualMotoRacing;
                            break;
                        case "VS186":
                            _productName = ViewResources.VirtualTennis;
                            break;
                        case "CS":
                            _productName = ViewResources.RNGCasino;
                            break;
                        case "BG":
                            _productName = ViewResources.Bingo;
                            break;
                        case "KN":
                            _productName = ViewResources.Keno;
                            break;
                        case "VS183":
                            _productName = ViewResources.VirtualSpeedway;
                            break;
                        case "VS185":
                            _productName = ViewResources.VirtualCycling;
                            break;                        
                        default:
                            _productName = _productCode;
                            break;
                    }
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