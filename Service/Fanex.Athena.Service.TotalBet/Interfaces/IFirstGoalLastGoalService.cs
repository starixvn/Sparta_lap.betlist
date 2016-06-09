// -----------------------------------------------------------------------
// <copyright file="IFirstGoalLastGoalService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IFirstGoalLastGoalService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel, out FirstGoalLastGoal totalData);
    }
}