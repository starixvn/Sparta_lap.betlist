namespace Fanex.Athena.Model.BetList.DTO
{
    public class ExchangeBetListBase
    {
        public string UserId { get; set; }

        public byte Currency { get; set; }

        public string MarketId { get; set; }

        public string MarketName { get; set; }

        public decimal WinLost { get; set; }

        public decimal Commission { get; set; }
    }
}
