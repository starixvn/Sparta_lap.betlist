// --------------------------------------------------------------------------
// <copyright file="IOddEvenDrawNoDrawDataAccess.cs" company="Nexcel Solutions Vietnam">
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
    /// Odd/Even 1X2 and Draw/No Draw interface.
    /// </summary>
    public interface IOddEvenDrawNoDrawDataAccess
    {
        IList<OddEvenDrawNoDraw> GetOddEventDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel);
    }
}