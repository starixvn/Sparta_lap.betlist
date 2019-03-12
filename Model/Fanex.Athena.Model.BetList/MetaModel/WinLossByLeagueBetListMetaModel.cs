namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class WinLossByLeagueBetListMetaModel : BetListExcelMetaModel
    {
        public bool ShowWin { get; set; }

        public bool ShowLoss { get; set; }

        public bool ShowDraw { get; set; }

        public decimal Odds { get; set; }
    }
}