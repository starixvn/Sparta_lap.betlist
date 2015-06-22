// -----------------------------------------------------------------------
// <copyright file="FirstGoalLastGoalViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

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