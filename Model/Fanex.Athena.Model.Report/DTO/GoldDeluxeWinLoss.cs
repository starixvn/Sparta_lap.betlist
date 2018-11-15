namespace Fanex.Athena.Model.Report.DTO
{
    public class GoldDeluxeWinLoss : ThirdPartyWinLoss
    {
        public decimal TipsAmount { get; set; }

        public decimal TipsShare { get; set; }

        public decimal AmountDue { get; set; }
    }
}