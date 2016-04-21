// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceViewModel.cs" company="Microsoft">
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
    /// AgCasinoWinLossViewModel class.
    /// </summary>
    public class AgCasinoBalanceViewModel : BaseViewModel
    {
        public IList<AgCasinoBalance> MainResult { get; set; }

        public IDictionary<string, AgCasinoBalance> SummaryTotal { get; set; }

        public AgCasinoBalanceMetaModel Param { get; set; }
    }
}