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
    using Model.Core.ViewModel;

    /// <summary>
    /// WinLossByProductViewModel class.
    /// </summary>
    public class WinLossByProductViewModel : BaseViewModel
    {
        public IList<WinLossByProduct> MainResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Reviewed. Suppression is OK here.")]
        public IDictionary<string, IList<WinLossByProduct>> SummaryTotal { get; set; }

        public WinLossByProductMetaModel Param { get; set; }
    }
}