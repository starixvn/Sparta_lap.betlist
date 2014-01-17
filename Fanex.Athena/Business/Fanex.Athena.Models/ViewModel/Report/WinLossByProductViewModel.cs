// -----------------------------------------------------------------------
// <copyright file="WinLossByProductViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// WinLossByProductViewModel class.
    /// </summary>
    public class WinLossByProductViewModel : BaseViewModel
    {
        public IList<WinLossByProduct> MainResult { get; set; }

        public IDictionary<string, IList<WinLossByProduct>> SummaryTotal { get; set; }

        public WinLossByProductMetaModel Param { get; set; }
    }
}