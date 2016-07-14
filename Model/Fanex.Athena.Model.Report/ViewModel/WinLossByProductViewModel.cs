// -----------------------------------------------------------------------
// <copyright file="WinLossByProductViewModel.cs" company="Microsoft">
// Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
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