// -----------------------------------------------------------------------
// <copyright file="ColossusBetWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    public class ColossusBetWinLossViewModel : BaseViewModel
    {
        public IList<ColossusBetWinLoss> MainResult { get; set; }

        public IDictionary<string, ColossusBetWinLoss> SummaryTotal { get; set; }

        public ThirdPartyWinLossMetaModel Param { get; set; }
    }
}