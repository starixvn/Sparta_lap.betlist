namespace Fanex.Athena.Model.BetList.DTO
{
    using System;

    public class ExchangeBetListEntity : ExchangeBetListBase
    {
        public long BetId { get; set; }

        public int EventId { get; set; }

        public string EventTypeName { get; set; }

        public string EventName { get; set; }

        public string Selection { get; set; }

        public DateTime SettledDate { get; set; }

        public decimal MatchAvgOdds { get; set; }

        public decimal MatchStake { get; set; }
    }
}