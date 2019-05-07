// --------------------------------------------------------------------------
// <copyright file="IFirstGoalLastGoalDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Threading.Tasks;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IFirstGoalLastGoalDataAccess
    {
        Task<ReportResultModel<FirstGoalLastGoal>> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel);
    }
}