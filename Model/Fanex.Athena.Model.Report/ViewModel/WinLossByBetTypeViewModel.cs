// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
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

        public string BalanceUpdate { get; set; }

        public int ColumnCount { get; set; }

        public WinLossByBetType Total { get; set; }
    }
}