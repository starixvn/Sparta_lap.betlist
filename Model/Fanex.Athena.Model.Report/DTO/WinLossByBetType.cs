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
        public int BetType { get; set; }

        public string BetTypeName { get; set; }

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
    }
}