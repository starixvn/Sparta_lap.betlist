// -----------------------------------------------------------------------
// <copyright file="WinLossByLeague.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    public class WinLossByLeague
    {
        public int Index { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public string HomeName { get; set; }

        public int AwayId { get; set; }

        public string AwayName { get; set; }

        public int BetCount { get; set; }

        public decimal BACommission { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public int CustomerId { get; set; }

        public decimal TurnOver { get; set; }

        public decimal LicenseeWL { get; set; }

        public decimal AmountDue { get; set; }

        public decimal CustomerWinLoss { get; set; }
    }
}