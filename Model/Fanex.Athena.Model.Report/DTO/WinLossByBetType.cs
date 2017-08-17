// -----------------------------------------------------------------------
// <copyright file="WinLossByBetType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using Core.DTO.Common;
    using Fanex.Athena.Model.Core.Helper;
    using Resources;

    public class WinLossByBetType : ICustomerWinLoss
    {
        private string _betTypeName;

        public int BetType { get; set; }

        public string BetTypeName
        {
            get
            {
                if (_betTypeName == null)
                {
                    _betTypeName = CacheRef.GetBettype(BetType, LiveIndicator) + GetSportName(BetType);
                }

                return _betTypeName;
            }
        }

        public decimal Turnover { get; set; }

        public decimal CustomerWinLoss { get; set; }

        public decimal CustomerComm { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal BACommission { get; set; }

        public decimal RacingRevenue { get; set; }

        public decimal RBComm { get; set; }

        public decimal RacingWinLoss
        {
            get { return RacingRevenue + RBComm; }
        }

        public decimal AmtDueToBA { get; set; }

        public decimal AmtDueToRacing { get; set; }

        public bool LiveIndicator { get; set; }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }

        private static string GetSportName(int bettype)
        {
            if (ProductValidator.IsValidBettype(Product.VS, bettype))
            {
                return " (" + ViewResources.ProductVS + ")";
            }

            if (ProductValidator.IsValidBettype(Product.VS2, bettype))
            {
                return " (" + ViewResources.ProductVS2 + ")";
            }

            return string.Empty;
        }
    }
}