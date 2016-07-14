// -----------------------------------------------------------------------
// <copyright file="IPlatformWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    public interface IPlatformWinLossDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel, out PlatformWinLossMember totalData);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel, out PlatformWinLossSummary totalData);
    }
}