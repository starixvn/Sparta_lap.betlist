// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    public class AgCasinoWinLossViewModel : BaseViewModel
    {
        public IList<AgCasinoWinLoss> MainResult { get; set; }

        public IDictionary<string, AgCasinoWinLoss> SummaryTotal { get; set; }

        public AgCasinoWinLossMetaModel Param { get; set; }
    }
}