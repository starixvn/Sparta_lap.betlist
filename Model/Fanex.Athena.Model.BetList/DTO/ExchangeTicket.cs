namespace Fanex.Athena.Model.BetList.DTO
{
    using System;

    public class ExchangeTicket
    {
        ////public int MarketId { get; set; }

        public string Market { get; set; }

        public int BetId { get; set; }

        public string Selection { get; set; }

        ////public int TypeId { get; set; }

        public string Type { get; set; }

        public DateTime BetPlaced { get; set; }

        public decimal OddsAvg { get; set; }

        public decimal Stake { get; set; }

        public decimal WinLoss { get; set; }

        public decimal WlComm { get; set; }

        public decimal LicWl { get; set; }

        public decimal LicWlComm { get; set; }
    }
}