// --------------------------------------------------------------------------
// <copyright file="BetListExcel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.BetList
{
    using System.Collections.Generic;
    using Excel.Base.Entities;

    public class BetListExcel : BaseBetList
    {
        public BetListExcel(ShowingOption showingOption, int licenseeId, IList<int> hiddenListColumn, bool isShowSumStake = false)
            : base(showingOption, licenseeId, hiddenListColumn, isShowSumStake)
        {
        }
    }
}