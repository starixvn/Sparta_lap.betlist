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
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    /// <summary>
    /// Money Line interface.
    /// </summary>
    public interface IMoneyLineDataAccess
    {
        IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel);
    }
}