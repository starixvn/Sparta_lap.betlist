// -----------------------------------------------------------------------
// <copyright file="IPlaytechCasinoService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;

    /// <summary>
    /// PLAYTECH Casino Service.
    /// </summary>
    public interface IPlaytechCasinoService
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetMainWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetAgentWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", 
            Justification = "Reviewed. Suppression is OK here.")]
        IList<PlaytechCasino> GetMemberWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp);
    }
}