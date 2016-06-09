// --------------------------------------------------------------------------
// <copyright file="IHomeDrawAwayNoBetDataAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.18">Created</change>
// </history>

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Home Draw Away Not Bet Interface.
    /// </summary>
    public interface IHomeDrawAwayNoBetDataAccess
    {
        IList<HomeDrawAwayNoBet> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel);
    }
}