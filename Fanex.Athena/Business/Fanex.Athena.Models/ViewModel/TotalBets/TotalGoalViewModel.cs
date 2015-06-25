// -----------------------------------------------------------------------
// <copyright file="TotalGoalViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.TotalBets
{
    using System.Collections.Generic;
    using DTO.TotalBets;
    using MetaModel.TotalBets;

    /// <summary>
    /// TotalGoalViewModel class.
    /// </summary>
    public class TotalGoalViewModel : TotalBetsBaseViewModel
    {
        public TotalGoalMetaModel Param { get; set; }

        public IList<TotalGoal> MainData { get; set; }

        public TotalGoal TotalData { get; set; }
    }
}