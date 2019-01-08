namespace Fanex.Athena.Service.Result.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;

    [System.CLSCompliant(false)]
    public interface IResultService
    {
        IList<Outright> GetOutRight(ResultMetalModel metal);

        #region ResultEngine

        MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting);

        string HtmlGameDetailParser(dynamic data, ResultSetting setting);

        string HtmlParser<T>(IList<T> dataList, ResultSetting setting);

        #endregion ResultEngine

        #region Filter

        IList<Sport> GetSports(DateTime dateValue, int licenseeId);

        IList<League> GetLeagues(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        IList<Sportsbook> GetMatches(ResultMetalModel metalModel);

        IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel);

        IList<Happy5> GetHappy5Matches(ResultMetalModel metalModel);

        IList<Racing> GetVirtualSportMatches(ResultMetalModel metalModel);

        #endregion Report

        #region Popup

        Sportsbook GetSportsbookGameDetail(int matchId);

        IList<Sportsbook> GetPopUpMatch(ResultPopupMetalModel metalModel);

        IList<Racing> GetPopUpVirtualSportMatch(ResultPopupMetalModel metalModel);

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

        IList<Happy5> GetPopUpHappy5Matches(int matchId);

        #endregion Popup
    }
}