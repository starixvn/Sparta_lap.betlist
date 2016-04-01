// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMobileWinLossService.cs" company="Nexcel">
// The  mobile win loss service.
// </copyright>
// <summary>
//   The  mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     The  mobile win loss service.
    /// </summary>
    public interface IMobileWinLossService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<MobileWinLossSummary> GetSummaryData(MobileWinLossMetaModel metaModel, out MobileWinLossSummary totalData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<MobileWinLossMember> GetMemberData(MobileWinLossMetaModel metaModel, out Dictionary<string, MobileWinLossMember> summaryTotal);
    }
}