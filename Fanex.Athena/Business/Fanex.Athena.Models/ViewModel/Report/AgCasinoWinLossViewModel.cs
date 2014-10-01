// -----------------------------------------------------------------------
// <copyright file="AgCasinoWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// AgCasinoWinLossViewModel class.
    /// </summary>
    public class AgCasinoWinLossViewModel : BaseViewModel
    {
        public IList<AgCasinoWinLoss> MainResult { get; set; }

        public IDictionary<string, AgCasinoWinLoss> SummaryTotal { get; set; }

        public AgCasinoWinLossMetaModel Param { get; set; }
    }
}