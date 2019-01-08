namespace Fanex.Athena.Model.Report.DTO
{
    using System;

    public class CasinoTurnover
    {
        public int Index { get; set; }

        public int RowSpan { get; set; }

        public DateTime Date { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public string Product { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal ValidBetAmount { get; set; }

        public decimal CustomerWL { get; set; }
    }
}
