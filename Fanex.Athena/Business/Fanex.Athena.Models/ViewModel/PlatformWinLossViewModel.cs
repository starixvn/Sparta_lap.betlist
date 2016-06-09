// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using DTO;
    using MetaModel;
    using Model.Core.ViewModel;

    public class PlatformWinLossViewModel : BaseViewModel
    {
        public IList<PlatformWinLossMember> MainResult { get; set; }

        public PlatformWinLossMember TotalResult { get; set; }

        public PlatformWinLossMetaModel Param { get; set; }

        public int Colspan { get; set; }
    }
}