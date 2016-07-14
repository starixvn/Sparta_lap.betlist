// -----------------------------------------------------------------------
// <copyright file="TotalGoal.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    /// <summary>
    /// TotalGoal class.
    /// </summary>
    public class TotalGoal : TotalBetsBase
    {
        public decimal Total01 { get; set; }

        public decimal Total23 { get; set; }

        public decimal Total46 { get; set; }

        public decimal Total7Over { get; set; }

        public decimal? TotalFirstHalf01 { get; set; }

        public decimal? TotalFirstHalf23 { get; set; }

        public decimal? Total4Over { get; set; }
    }
}