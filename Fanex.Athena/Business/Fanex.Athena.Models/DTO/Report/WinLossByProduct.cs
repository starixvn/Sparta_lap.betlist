// -----------------------------------------------------------------------
// <copyright file="WinLossByProduct.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using Fanex.Athena.Models.ViewModel;
    using System;
    using System.Diagnostics.CodeAnalysis;

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

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed.")]
        private static string GetProductName(string productCode)
        {
            switch (productCode.ToUpper())
            {
                case "BA":
                    return ViewResources.ProductBA;

                case "LC":
                    return ViewResources.ProductLC;

                case "NG":
                    return ViewResources.ProductNG;

                case "RB":
                    return ViewResources.ProductRB;

                case "SB":
                    return ViewResources.Sportsbook;

                case "MGCLC":
                    return ViewResources.ProductMGCLC;

                case "MGCRNG":
                    return ViewResources.ProductMGCRNG;

                case "VS":
                    return ViewResources.ProductVS;

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
                    return ViewResources.ProductCS;

                case "BG":
                    return ViewResources.ProductBG;

                case "KN":
                    return ViewResources.ProductKN;

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