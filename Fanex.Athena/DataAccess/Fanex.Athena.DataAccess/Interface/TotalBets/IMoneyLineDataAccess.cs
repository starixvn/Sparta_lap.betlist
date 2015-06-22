// --------------------------------------------------------------------------
// <copyright file="IMoneyLineDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Money Line interface.
    /// </summary>
    public interface IMoneyLineDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel, out MoneyLine totalData);
    }
}