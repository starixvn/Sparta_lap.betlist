// --------------------------------------------------------------------------
// <copyright file="IOutrightDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
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
    /// Outright Interface.
    /// </summary>
    public interface IOutrightDataAccess
    {
        IList<Outright> GetOutrightDataList(OutrightMetaModel metaModel);
    }
}