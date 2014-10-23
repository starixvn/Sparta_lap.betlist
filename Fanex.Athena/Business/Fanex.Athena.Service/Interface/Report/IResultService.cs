// -----------------------------------------------------------------------
// <copyright file="IResultService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Report
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Result.Core.DTO;

    public interface IResultService
    {
        /// <summary>
        /// Get out right.
        /// </summary>
        /// <param name="metal">The metal model.</param>
        /// <returns>The out right list.</returns>
        IList<OutRight> GetOutRight(ResultMetalModel metal);

        #region Filter

        /// <summary>
        ///  Get sports.
        /// </summary>
        /// <param name="dateValue">The date time.</param>
        /// <param name="licenseeId">The licensee Id.</param>
        /// <returns>The sport list.</returns>
        IList<Sport> GetSports(DateTime dateValue, int licenseeId);

        /// <summary>
        /// Get leagues.
        /// </summary>
        /// <param name="dateValue">The date time.</param>
        /// <param name="sportId">The sport id.</param>
        /// <returns>The leagues.</returns>
        IList<League> GetLeagues(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        /// <summary>
        /// Get match.
        /// </summary>
        /// <param name="metalModel"> The metal model.</param>
        /// <returns>The match info.</returns>
        IList<Sportbook> GetMatches(ResultMetalModel metalModel);

        /// <summary>
        /// Get live casino match.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The match info.</returns>
        IList<LiveCasino> GetLiveCasinoMatches(ResultMetalModel metalModel);

        /// <summary>
        /// Get number game match.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The match info.</returns>
        IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel);

        /// <summary>
        /// Get racing match.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <returns>The match info.</returns>
        IList<Racing> GetRacingMatches(ResultMetalModel metalModel);

        #endregion Report

        #region Popup

        /// <summary>
        /// Get match for pop up pages.
        /// Only used for sport-book and racing.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="bettypeId">Bet type id.</param>
        /// <param name="betId">The bet id.</param>
        /// <returns>The match info.</returns>
        IList<Sportbook> GetPopUpMatch(int matchId, int licenseeId, int userId, int bettypeId, long? betId);

        /// <summary>
        /// Get live casino match for pop up page.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="bettypeId">Bet type id.</param>
        /// <param name="betId">The bet id.</param>
        /// <returns>The match info.</returns>
        IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int licenseeId, int userId, int bettypeId, long? betId);

        /// <summary>
        /// Get the racing match for pop up page.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="raceNo">The race no.</param>
        /// <param name="licenseeId">The licensee id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="bettypeId">Bet type id.</param>
        /// <param name="betId">The bet id.</param>
        /// <returns>The match info.</returns>
        IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int licenseeId, int userId, int bettypeId, long? betId);

        /// <summary>
        /// Select matches from BetTransM by reference no.
        /// For view MixParlay result.
        /// </summary>
        /// <param name="refNo">The reference no.</param>
        /// <returns>The parlay list.</returns>
        IList<Parlay> GetPopUpParlayMatch(long refNo);

        /// <summary>
        ///  Get match result of number game.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <returns>The match info.</returns>
        IList<NumberGame> GetPopUpNumberGameMatchs(int matchId);

        #endregion Popup
    }
}