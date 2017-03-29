// -----------------------------------------------------------------------
// <copyright file="CashOutWinLossViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    public class CashOutWinLossViewModel : BaseViewModel
    {
        public IList<CashOutWinLoss> MainResult { get; set; }

        public IDictionary<string, CashOutWinLoss> SummaryTotal { get; set; }

        public CashOutWinLossMetaModel Param { get; set; }
    }
}