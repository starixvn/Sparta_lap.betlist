// -----------------------------------------------------------------------
// <copyright file="IWinLossByProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IWinLossByProductDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLossByProduct> GetWinlossByProducts(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLossByProduct> GetWinlossByProductAgents(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<WinLossByProduct> GetWinlossByProductCustomers(
            WinLossByProductMetaModel metaModel,
            out IDictionary<string, IList<WinLossByProduct>> summaryTotal);
    }
}