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
        IEnumerable<WinLossByProduct> GetWinlossByProducts(
            WinLossByProductMetaModel metaModel,
            out IEnumerable<WinLossByProduct> totalData,
            out IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal,
            out IResultInfo resultInfo);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WinLossByProduct> GetWinlossByProductAgents(
            WinLossByProductMetaModel metaModel,
            out IEnumerable<WinLossByProduct> totalData,
            out IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IEnumerable<WinLossByProduct> GetWinlossByProductCustomers(
            WinLossByProductMetaModel metaModel,
            out IEnumerable<WinLossByProduct> totalData,
            out IDictionary<string, IEnumerable<WinLossByProduct>> summaryTotal);
    }
}