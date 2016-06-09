// -----------------------------------------------------------------------
// <copyright file="WinLossByBetType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using Fanex.Athena.Model.Core.Helper;
    using Fanex.Athena.Models.Helper;
    using Resources;

    /// <summary>
    /// WinLossByBetType class.
    /// </summary>
    public class WinLossByBetType : ICustomerWinLoss
    {
        private string _betTypeName;
        private int _betType;
        private bool _liveIndicator;

        public int BetType
        {
            get { return _betType; }
            set { _betType = value; }
        }

        public string BetTypeName
        {
            get
            {
                if (_betTypeName == null)
                {
                    _betTypeName = CacheRef.GetBettype(_betType, _liveIndicator) +
                        (Validator.IsValidBettype("VS", _betType) ? " (" + ViewResources.ProductVS + ")" : string.Empty);
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

        public bool LiveIndicator
        {
            get { return _liveIndicator; }
            set { _liveIndicator = value; }
        }

        public decimal CustomerTotal
        {
            get { return CustomerWinLoss + CustomerComm; }
        }
    }
}