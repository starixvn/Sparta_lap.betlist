// -----------------------------------------------------------------------
// <copyright file="IMobileWinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    ///     The i mobile win loss data access.
    /// </summary>
    public interface IMobileWinLossDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<MobileWinLossMember> GetMemberData(MobileWinLossMetaModel metaModel, out MobileWinLossMember totalData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<MobileWinLossSummary> GetSummaryData(MobileWinLossMetaModel metaModel, out MobileWinLossSummary totalData);
    }
}