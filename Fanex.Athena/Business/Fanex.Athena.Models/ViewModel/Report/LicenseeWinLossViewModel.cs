// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// LicenseeWinLossViewModel class.
    /// </summary>
    public class LicenseeWinLossViewModel : BaseViewModel
    {
        public IList<LicenseeWinLoss> MainResult { get; set; }

        public IDictionary<string, LicenseeWinLoss> SummaryTotal { get; set; }

        public LicenseeWinLossMetaModel Param { get; set; }
    }
}