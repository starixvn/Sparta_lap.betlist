// --------------------------------------------------------------------------
// <copyright file="IRacingDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.Interface.TotalBets
{
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Racing interface.
    /// </summary>
    public interface IRacingDataAccess
    {
        Racing GetRacingDataList(RacingMetaModel metaModel);
    }
}