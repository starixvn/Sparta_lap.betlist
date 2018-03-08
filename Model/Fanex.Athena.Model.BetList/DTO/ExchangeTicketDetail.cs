namespace Fanex.Athena.Model.BetList.DTO
{
    using System;
    using Core.DataDomain;

    public class ExchangeTicketDetail
    {
        public DateTime BetTaken { get; set; }

        public decimal OddsReq { get; set; }

        public decimal Stake { get; set; }

        public string Liability { get; set; }

        public decimal OddsMatched { get; set; }

        public string BetTakenDateString => BetTaken.ToString(FormatKey.DateTimeFormat);
    }
}