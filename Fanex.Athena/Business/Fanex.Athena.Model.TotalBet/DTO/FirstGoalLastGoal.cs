// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoal.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.DTO
{
    /// <summary>
    /// FirstGoalLastGoal class.
    /// </summary>
    public class FirstGoalLastGoal : TotalBetsBase
    {
        public decimal HomeFirstGoal { get; set; }

        public decimal HomeLastGoal { get; set; }

        public decimal AwayFirstGoal { get; set; }

        public decimal AwayLastGoal { get; set; }

        public decimal NoGoal { get; set; }

        public decimal? HomeFirstHalfFirstGoal { get; set; }

        public decimal? HomeFirstHalfLastGoal { get; set; }

        public decimal? AwayFirstHalfFirstGoal { get; set; }

        public decimal? AwayFirstHalfLastGoal { get; set; }

        public decimal? FirstHalfNoGoal { get; set; }
    }
}