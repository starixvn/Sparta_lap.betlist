// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
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