// --------------------------------------------------------------------------
// <copyright file="IHomeDrawAwayNoBetDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Home Draw Away Not Bet Interface.
    /// </summary>
    public interface IHomeDrawAwayNoBetDataAccess
    {
        Task<IList<HomeDrawAwayNoBet>> GetHomeDrawAwayNoBetDataList(HomeDrawAwayNoBetMetaModel metaModel);
    }
}