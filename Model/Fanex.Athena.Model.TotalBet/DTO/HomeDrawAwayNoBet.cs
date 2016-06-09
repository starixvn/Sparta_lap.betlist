// -----------------------------------------------------------------------
// <copyright file="HomeDrawAwayNoBet.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    /// <summary>
    /// HomeDrawAwayNoBet class.
    /// </summary>
    public class HomeDrawAwayNoBet : TotalBetsBase
    {
        public decimal HomeOddsHomeNoBet { get; set; }

        public decimal AwayOddsHomeNoBet { get; set; }

        public decimal HomeOddsDrawNoBet { get; set; }

        public decimal AwayOddsDrawNoBet { get; set; }

        public decimal HomeOddsAwayNoBet { get; set; }

        public decimal AwayOddsAwayNoBet { get; set; }

        public int BetCountHomeNoBet { get; set; }

        public int BetCountDrawNoBet { get; set; }

        public int BetCountAwayNoBet { get; set; }

        public decimal HomeNoBet
        {
            get { return HomeOddsHomeNoBet - AwayOddsHomeNoBet; }
        }

        public decimal DrawNoBet
        {
            get { return HomeOddsDrawNoBet - AwayOddsDrawNoBet; }
        }

        public decimal AwayNoBet
        {
            get { return HomeOddsAwayNoBet - AwayOddsAwayNoBet; }
        }

        public decimal RunningStakeHomeNoBet
        {
            get { return HomeOddsHomeNoBet + AwayOddsHomeNoBet; }
        }

        public decimal RunningStakeDrawNoBet
        {
            get { return HomeOddsDrawNoBet + AwayOddsDrawNoBet; }
        }

        public decimal RunningStakeAwayNoBet
        {
            get { return HomeOddsAwayNoBet + AwayOddsAwayNoBet; }
        }
    }
}