// --------------------------------------------------------------------------
// <copyright file="IOutrightDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Outright Interface.
    /// </summary>
    public interface IOutrightDataAccess
    {
        IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel);
    }
}