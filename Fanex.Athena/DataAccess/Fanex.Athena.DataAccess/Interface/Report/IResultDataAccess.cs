// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Result.Core.DTO;

    public interface IResultDataAccess
    {
        IList<OutRight> GetOutRight(ResultMetalModel metal);

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

        IList<T> GetPopUpMatches<T>(int matchId, int raceNo, int licenseeId, int userId);

        IList<Parlay> GetPopUpParalayMatch(long refNo);

        IList<NumberGame> GetPopUpNumberGameMatch(int matchId);

        #endregion Popup
    }
}