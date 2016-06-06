// -----------------------------------------------------------------------
// <copyright file="HalfTimeFullTime.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    /// <summary>
    /// HalfTimeFullTime class.
    /// </summary>
    public class HalfTimeFullTime : TotalBetsBase
    {
        public decimal HomeHome { get; set; }

        public decimal HomeDraw { get; set; }

        public decimal HomeAway { get; set; }

        public decimal DrawHome { get; set; }

        public decimal DrawDraw { get; set; }

        public decimal DrawAway { get; set; }

        public decimal AwayHome { get; set; }

        public decimal AwayDraw { get; set; }

        public decimal AwayAway { get; set; }
    }
}