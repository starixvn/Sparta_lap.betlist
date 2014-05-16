// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoal.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.TotalBets
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

        public object HomeFirstHalfFirstGoal { get; set; }

        public object HomeFirstHalfLastGoal { get; set; }

        public object AwayFirstHalfFirstGoal { get; set; }

        public object AwayFirstHalfLastGoal { get; set; }

        public object FirstHalfNoGoal { get; set; }
    }
}