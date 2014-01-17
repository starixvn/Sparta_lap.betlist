// -----------------------------------------------------------------------
// <copyright file="TotalGoal.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
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
    }
}