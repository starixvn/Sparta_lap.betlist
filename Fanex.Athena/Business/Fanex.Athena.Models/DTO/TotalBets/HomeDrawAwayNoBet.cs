// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBet.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
{
    /// <summary>
    /// HomeDrawAwayNoBet class.
    /// </summary>
    public class HomeDrawAwayNoBet : TotalBetsBase
    {
        public decimal HomeNoBet { get; set; }

        public decimal DrawNoBet { get; set; }

        public decimal AwayNoBet { get; set; }

        public int BetCountHomeNoBet { get; set; }

        public int BetCountDrawNoBet { get; set; }

        public int BetCountAwayNoBet { get; set; }
    }
}