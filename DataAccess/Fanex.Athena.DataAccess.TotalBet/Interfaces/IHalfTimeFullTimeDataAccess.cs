// --------------------------------------------------------------------------
// <copyright file="IHalfTimeFullTimeDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    /// Half Time Full Time Interface.
    /// </summary>
    public interface IHalfTimeFullTimeDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewed.")]
        IList<HalfTimeFullTime> GetHalfTimeFullTimeDataList(HalfTimeFullTimeMetaModel metaModel, out HalfTimeFullTime totalData);
    }
}