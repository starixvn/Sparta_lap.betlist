// -----------------------------------------------------------------------
// <copyright file="TotalGoalViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;

    /// <summary>
    /// TotalGoalViewModel class.
    /// </summary>
    public class TotalGoalViewModel : TotalBetsBaseViewModel
    {
        public TotalGoalMetaModel Param { get; set; }

        public IEnumerable<TotalGoal> MainData { get; set; }

        public TotalGoal TotalData { get; set; }
    }
}