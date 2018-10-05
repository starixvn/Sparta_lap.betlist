// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// FirstGoalLastGoalViewModel class.
    /// </summary>
    public class FirstGoalLastGoalViewModel : TotalBetsBaseViewModel
    {
        public FirstGoalLastGoalMetaModel Param { get; set; }

        public IEnumerable<FirstGoalLastGoal> MainData { get; set; }

        public FirstGoalLastGoal TotalData { get; set; }
    }
}