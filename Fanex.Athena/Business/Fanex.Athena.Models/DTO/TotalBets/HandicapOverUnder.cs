// -----------------------------------------------------------------------
// <copyright file="HandicapOverUnder.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    /// <summary>
    /// HandicapOverUnder class.
    /// </summary>
    public class HandicapOverUnder : TotalBetsBase
    {
        public decimal FullTimeHandicap { get; set; }

        public decimal FullTimeOverUnder { get; set; }

        public decimal FirstTimeHandicap { get; set; }

        public decimal FirstTimeOverUnder { get; set; }

        public int BetCountHandicap { get; set; }

        public int BetCountOverUnder { get; set; }

        public int BetCountHandicap1StHalf { get; set; }

        public int BetCountOverUnder1StHalf { get; set; }
    }
}