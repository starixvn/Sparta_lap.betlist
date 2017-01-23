// --------------------------------------------------------------------------
// <copyright file="BetListExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.BetList
{
    using System.Collections.Generic;

    using Base;
    using Excel.Base.Entities;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class BetListExcel : BaseBetList
    {
        public BetListExcel(ShowingOption showingOption, bool is12Bet, IList<int> hiddenListColumn, bool isShowSumStake = false)
            : base(showingOption, is12Bet, hiddenListColumn, isShowSumStake)
        {
        }
    }
}