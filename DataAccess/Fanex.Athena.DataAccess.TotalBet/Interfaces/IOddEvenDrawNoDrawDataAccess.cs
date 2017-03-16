// --------------------------------------------------------------------------
// <copyright file="IOddEvenDrawNoDrawDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Odd/Even 1X2 and Draw/No Draw interface.
    /// </summary>
    public interface IOddEvenDrawNoDrawDataAccess
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<OddEvenDrawNoDraw> GetOddEventDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel, out OddEvenDrawNoDraw totalData);
    }
}