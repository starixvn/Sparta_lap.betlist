// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    public class MicroGamingWinLossViewModel : BaseViewModel
    {
        public IEnumerable<MicroGamingWinLoss> MainResult { get; set; }

        public IDictionary<string, MicroGamingWinLoss> SummaryTotal { get; set; }

        public ThirdPartyWinLossMetaModel Param { get; set; }
    }
}