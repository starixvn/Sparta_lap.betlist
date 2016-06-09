// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Interface.Report;

    /// <summary>
    /// PLAYTECH Casino Service.
    /// </summary>
    public class PlaytechCasinoService : IPlaytechCasinoService
    {
        private readonly IPlaytechCasinoDataAccess _playtechCasinoDataAccess;

        public PlaytechCasinoService()
        {
            _playtechCasinoDataAccess = new PlaytechCasinoDataAccess();
        }

        public IList<PlaytechCasino> GetMainWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp)
        {            
            return metaModel.IsValid
                ? _playtechCasinoDataAccess.GetMainWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp)
                : new List<PlaytechCasino>();
        }

        public IList<PlaytechCasino> GetAgentWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp)
        {
            return metaModel.IsValid
                ? _playtechCasinoDataAccess.GetAgentWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp)
                : new List<PlaytechCasino>();
        }

        public IList<PlaytechCasino> GetMemberWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            ref PlaytechCasino totalInGbp)
        {
            return metaModel.IsValid
                ? _playtechCasinoDataAccess.GetMemberWinLoss(metaModel, ref totalInEur, ref totalInUsd, ref totalInGbp)
                : new List<PlaytechCasino>();
        }
    }
}
