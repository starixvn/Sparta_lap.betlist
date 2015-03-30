// -----------------------------------------------------------------------
// <copyright file="VoidbridgeWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class VoidbridgeWinLossViewModel : BaseViewModel
    {
        public IList<VoidbridgeWinLoss> MainResult { get; set; }

        public IDictionary<string, VoidbridgeWinLoss> SummaryTotal { get; set; }

        public VoidbridgeWinLossMetaModel Param { get; set; }
    }
}