namespace Fanex.Athena.Model.BetList.DTO
{
    public class ExchangeSumWinLoss
    {
        public string UserId { get; set; }

        public int EventId { get; set; }

        public string EventTypeName { get; set; }

        public string EventName { get; set; }

        public string MarketId { get; set; }

        public string MarketName { get; set; }

        public decimal WinLost { get; set; }

        public decimal Commission { get; set; }
    }
}