// -----------------------------------------------------------------------
// <copyright file="WinLossByLeagueBetListDetailMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class WinLossByLeagueBetListMetaModel : BetListMetaModel
    {
        public bool ShowWin { get; set; }

        public bool ShowLoss { get; set; }

        public bool ShowDraw { get; set; }

        public decimal Odds { get; set; }
    }
}