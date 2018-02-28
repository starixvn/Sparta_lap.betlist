namespace Fanex.Athena.Model.BetList.DTO
{
    using System;

    public class ExchangeTicketResult
    {
        public DateTime BetTaken { get; set; }

        public decimal OddsReq { get; set; }

        public decimal Stake { get; set; }

        public decimal Liability { get; set; }

        public decimal OddsMacthed { get; set; }
    }
}