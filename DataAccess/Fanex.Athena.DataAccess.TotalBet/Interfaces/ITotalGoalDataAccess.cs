// --------------------------------------------------------------------------
// <copyright file="ITotalGoalDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Total Goal Interface.
    /// </summary>
    public interface ITotalGoalDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<TotalGoal> GetTotalGoalDataList(TotalGoalMetaModel metaModel, out TotalGoal totalData);
    }
}