// --------------------------------------------------------------------------
// <copyright file="BetListExcel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.BetList
{
    using System.Collections.Generic;

    using Base;
    using Entities;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetListExcel : BaseBetList
    {
        public BetListExcel(ShowingOption showingOption, bool is12Bet, IList<int> hiddenListColumn)
            : base(showingOption, is12Bet, hiddenListColumn)
        {
        }
    }
}