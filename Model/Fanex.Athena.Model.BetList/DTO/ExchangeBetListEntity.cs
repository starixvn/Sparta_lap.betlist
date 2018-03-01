namespace Fanex.Athena.Model.BetList.DTO
{
    using System;

    public class ExchangeBetListEntity : ExchangeBetListBase
    {
        public long BetId { get; set; }

        public string Type { get; set; }

        public string Selection { get; set; }

        public DateTime SettledDate { get; set; }

        public decimal MatchAvgOdds { get; set; }

        public decimal MatchStake { get; set; }

        public string BetPlaced => SettledDate.ToString("MM/dd/yyyy hh:mm:ss");
    }
}