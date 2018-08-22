// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeViewModel.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// WinLossByBetTypeViewModel class.
    /// </summary>
    public class WinLossByBetTypeViewModel : BaseViewModel
    {
        public IList<WinLossByBetType> MainResult { get; set; }

        public WinLossByBetTypeMetaModel Param { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int ColumnCount { get; set; }

        public WinLossByBetType Total { get; set; }
    }
}