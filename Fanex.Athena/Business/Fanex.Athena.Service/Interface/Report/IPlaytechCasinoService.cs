// -----------------------------------------------------------------------
// <copyright file="IPlaytechCasinoService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

    /// <summary>
    /// PLAYTECH Casion Service.
    /// </summary>
    public interface IPlaytechCasinoService
    {
        IList<PlaytechCasino> GetMainWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);

        IList<PlaytechCasino> GetAgentWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);

        IList<PlaytechCasino> GetMemberWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd);
    }
}
