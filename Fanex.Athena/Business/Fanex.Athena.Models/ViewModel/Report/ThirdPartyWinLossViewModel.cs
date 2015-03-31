// -----------------------------------------------------------------------
// <copyright file="ThirdPartyWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// ThirdPartyWinLossViewModel class.
    /// </summary>
    public class ThirdPartyWinLossViewModel : BaseViewModel
    {
        public IList<ThirdPartyWinLoss> MainResult { get; set; }

        public IDictionary<string, ThirdPartyWinLoss> SummaryTotal { get; set; }

        public ThirdPartyWinLossMetaModel Param { get; set; }
    }
}