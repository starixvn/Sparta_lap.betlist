namespace Fanex.Athena.Model.Report.DTO
{
    public class GoldDeluxeWinLoss : ThirdPartyWinLoss
    {
        public decimal TipAmount { get; set; }

        public decimal TipSharing { get; set; }

        public decimal AmountDue { get; set; }
    }
}