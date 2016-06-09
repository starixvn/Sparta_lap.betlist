// -----------------------------------------------------------------------
// <copyright file="AgCasinoBalanceViewModel.cs" company="Microsoft">
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
    /// AgCasinoWinLossViewModel class.
    /// </summary>
    public class AgCasinoBalanceViewModel : BaseViewModel
    {
        public IList<AgCasinoBalance> MainResult { get; set; }

        public IDictionary<string, AgCasinoBalance> SummaryTotal { get; set; }

        public AgCasinoBalanceMetaModel Param { get; set; }
    }
}