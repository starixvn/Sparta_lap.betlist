namespace Fanex.Athena.Model.BetList.DTO
{
    public class ExchangeTicketDetail
    {
        public string BetTaken { get; set; }

        public decimal OddsReq { get; set; }

        public decimal Stake { get; set; }

        public string Liability { get; set; }

        public decimal OddsMatched { get; set; }
    }
}