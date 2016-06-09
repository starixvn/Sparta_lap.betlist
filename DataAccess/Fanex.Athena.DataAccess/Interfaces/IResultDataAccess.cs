// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;

    [System.CLSCompliant(false)]
    public interface IResultDataAccess
    {
        IList<Outright> GetOutRight(ResultMetalModel metal);

        #region Filter

        IList<Sport> GetSports(DateTime dateValue, int licenseeId);

        IList<League> GetLeagues(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        IList<T> GetMatches<T>(ResultMetalModel metalModel);

        IList<Racing> GetRacingMatches(ResultMetalModel metalModel);

        IList<NumberGame> GetNumberGameMatches(ResultMetalModel metalModel);

        #endregion Report

        #region Popup

        IList<T> GetPopUpMatches<T>(int matchId, int raceNo, int licenseeId, int userId, int bettypeId, long? betId);

        IList<Parlay> GetPopUpParalayMatch(long refNo);

        IList<NumberGame> GetPopUpNumberGameMatch(int matchId);
        
        Sportsbook GetSportsbookGameDetail(int matchId);

        #endregion Popup
    }
}