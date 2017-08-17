namespace Fanex.Athena.Model.BetSettings.DTO
{
    public class DcsBetSetting
    {
        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public string SportId { get; set; }

        public int BetType { get; set; }
    }
}