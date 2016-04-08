// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using DTO.Report;
    using MetaModel.Report;

    public class PlatformWinLossViewModel : BaseViewModel
    {
        public IList<PlatformWinLossMember> MainResult { get; set; }

        public PlatformWinLossMember TotalResult { get; set; }

        public PlatformWinLossMetaModel Param { get; set; }

        public int Colspan { get; set; }
    }
}