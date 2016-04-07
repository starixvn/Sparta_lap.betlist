// -----------------------------------------------------------------------
// <copyright file="MobileWinLossViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using DTO.Report;
    using MetaModel.Report;

    /// <summary>
    /// MobileWinLossViewModel class.
    /// </summary>
    public class MobileWinLossViewModel : BaseViewModel
    {
        public IList<MobileWinLossMember> MainResult { get; set; }

        public MobileWinLossMember TotalResult { get; set; }

        public MobileWinLossMetaModel Param { get; set; }

        public int Colspan { get; set; }
    }
}