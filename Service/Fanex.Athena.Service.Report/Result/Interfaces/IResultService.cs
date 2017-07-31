namespace Fanex.Athena.Service.Result.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Result.Core.LiveCasino.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;

    [System.CLSCompliant(false)]
    public interface IResultService
    {
        /// <summary>
        /// Get out right.
        /// </summary>
        /// <param name="metal">The metal model.</param>
        /// <returns>The out right list.</returns>
        IList<Outright> GetOutRight(ResultMetalModel metal);

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
        IList<Sportsbook> GetMatches(ResultMetalModel metalModel);

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

        Sportsbook GetSportsbookGameDetail(int matchId);

        IList<Sportsbook> GetPopUpMatch(int matchId, int bettypeId, long? betId);

        IList<LiveCasino> GetPopUpLiveCasinoMatch(int matchId, int bettypeId, long? betId);

        IList<Racing> GetPopUpRacingMatch(int matchId, int raceNo, int bettypeId, long? betId);

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