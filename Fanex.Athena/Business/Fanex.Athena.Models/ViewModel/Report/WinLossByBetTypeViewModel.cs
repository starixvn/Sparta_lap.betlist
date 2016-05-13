// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeViewModel.cs" company="Nexcel">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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