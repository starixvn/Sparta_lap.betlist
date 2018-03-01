namespace Fanex.Athena.Model.BetList.DTO
{
    public class ExchangeBetList
    {
        public string UserId { get; set; }

        public int EvenId { get; set; }

        public string EvenTypeName { get; set; }

        public string EvenName { get; set; }

        public string MarketId { get; set; }

        public string MarketName { get; set; }

        public decimal WinLost { get; set; }

        public decimal Commission { get; set; }
    }
}