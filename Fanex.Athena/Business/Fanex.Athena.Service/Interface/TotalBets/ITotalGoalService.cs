// -----------------------------------------------------------------------
// <copyright file="ITotalGoalService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface ITotalGoalService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel, out TotalGoal totalData);
    }
}