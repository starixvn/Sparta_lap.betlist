// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalViewModel.cs" company="">
// warehouse winloss form parameters
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

        public IList<FirstGoalLastGoal> MainData { get; set; }

        public FirstGoalLastGoal TotalData { get; set; }
    }
}