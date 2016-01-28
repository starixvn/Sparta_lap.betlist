// -----------------------------------------------------------------------
// <copyright file="LiveReport.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report.LiveReports
{
    using System;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// LiveReport class.
    /// </summary>
    public class LiveReport : ICustomerWinLoss
    {
        private string _productName;
        private string _productCode;

        /// <summary>
        /// Gets or sets type.
        /// 0:current,1:product,2:sport,3 bet type.
        /// </summary>
        /// <value>The type value.</value>
        public byte ReportType
        {
            get;
            set;
        }

        public string ProductName
        {
            get
            {
                switch (ReportType)
                {
                    case 0:
                        return _productName;
                    case 1:
                        switch (_productCode.ToUpper())
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

                            case "VS":
                                return ViewResources.ProductVS;

                            case "CS":
                                return ViewResources.ProductCS;

                            case "BG":
                                return ViewResources.ProductBG;
                            default:
                                return _productName;
                        }

                    case 2:
                        var sportId = string.IsNullOrEmpty(_productCode) ? 0 : Convert.ToInt32(_productCode);
                        switch (sportId)
                        {
                            case 101:
                                return CacheRef.GetBettype((int)BetType.MixParlay);

                            case 102:
                                return CacheRef.GetBettype((int)BetType.CombinationMixParlay);

                            case 103:
                                return CacheRef.GetBettype((int)BetType.Outright);

                            default:
                                return CacheRef.GetSportName(_productCode);
                        }

                    case 3:
                        return CacheRef.GetBettype(Convert.ToInt32(_productCode), false);
                    default:
                        return _productName;
                }
            }

            set
            {
                _productName = value;
            }
        }

        public decimal TurnOver { get; set; }

        public int BetCount { get; set; }

        public decimal LicenseeWinLoss { get; set; }

        public decimal LicenseeComm { get; set; }

        public decimal AmountDue { get; set; }

        public string ProductCode
        {
            get { return _productCode; }
            set { _productCode = value; }
        }

        public decimal CustomerWinLoss
        {
            get;

            set;
        }

        public decimal CustomerComm
        {
            get;

            set;
        }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}