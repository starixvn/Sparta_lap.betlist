// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Model.Core.ViewModel;

    /// <summary>
    /// GoldDeluxeWinLossViewModel class.
    /// </summary>
    public class GoldDeluxeWinLossViewModel : BaseViewModel
    {
        public IList<GoldDeluxeWinLoss> MainResult { get; set; }

        public IDictionary<string, GoldDeluxeWinLoss> SummaryTotal { get; set; }

        public GoldDeluxeWinLossMetaModel Param { get; set; }
    }
}