// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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