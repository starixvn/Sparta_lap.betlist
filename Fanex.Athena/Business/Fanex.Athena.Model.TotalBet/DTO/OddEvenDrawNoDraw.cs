// -----------------------------------------------------------------------
// <copyright file="OddEvenDrawNoDraw.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    /// <summary>
    /// Odd/Even + 1X2 + Draw/NoDraw class.
    /// </summary>
    public class OddEvenDrawNoDraw : TotalBetsBase
    {
        public decimal OddEvenFt { get; set; }

        public decimal OneXTwoFt { get; set; }

        public decimal DrawNoDraw { get; set; }

        public decimal OddEvenFirstHalf { get; set; }

        public decimal OneXTwoFirstHalf { get; set; }

        public int BetCountOddEven { get; set; }

        public int BetCountOneXTwo { get; set; }

        public int BetCountDrawNoDraw { get; set; }

        public int BetCountOddEven1StHalf { get; set; }

        public int BetCountOneXTow1StHalf { get; set; }

        public int Bettype { get; set; }
    }
}