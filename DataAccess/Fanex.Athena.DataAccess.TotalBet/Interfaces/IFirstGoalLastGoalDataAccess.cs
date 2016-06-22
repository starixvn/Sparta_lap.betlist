// --------------------------------------------------------------------------
// <copyright file="IFirstGoalLastGoalDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
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
    /// First Goal Last Goal Interface.
    /// </summary>
    public interface IFirstGoalLastGoalDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<FirstGoalLastGoal> GetFirstGoalLastGoalDataList(FirstGoalLastGoalMetaModel metaModel, out FirstGoalLastGoal totalData);
    }
}