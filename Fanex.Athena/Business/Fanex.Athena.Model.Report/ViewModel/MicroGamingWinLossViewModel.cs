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

    /// <summary>
    /// MicroGamingWinLossViewModel class.
    /// </summary>
    public class MicroGamingWinLossViewModel : BaseViewModel
    {
        public IList<MicroGamingWinLoss> MainResult { get; set; }

        public IDictionary<string, MicroGamingWinLoss> SummaryTotal { get; set; }

        public MicroGamingWinLossMetaModel Param { get; set; }
    }
}