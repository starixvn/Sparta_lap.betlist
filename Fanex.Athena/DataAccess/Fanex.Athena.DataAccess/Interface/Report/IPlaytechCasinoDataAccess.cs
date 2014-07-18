// -----------------------------------------------------------------------
// <copyright file="IPlaytechCasinoDataAccess.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// Interface PLAYTECH Casino Data Access.
    /// </summary>
    public interface IPlaytechCasinoDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetMainWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetAgentWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference",
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetMemberWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);
    }
}
