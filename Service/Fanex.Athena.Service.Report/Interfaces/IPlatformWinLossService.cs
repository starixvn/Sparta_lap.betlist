// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlatformWinLossService.cs" company="">
// The  mobile win loss service.
// </copyright>
// <summary>
//   The  mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IPlatformWinLossService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel, out PlatformWinLossSummary totalData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel, out PlatformWinLossMember totalData);
    }
}