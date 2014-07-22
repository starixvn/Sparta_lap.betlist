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

    public interface IResultDataAccess
    {
        DataTable GetOutRight(ResultMetalModel metal);

        #region Filter

        IList<Sport> GetSports(DateTime dateValue, int licenseeId);

        IList<League> GetLeagues(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        DataTable GetMatchs(ResultMetalModel metalModel);

        DataTable GetNumberGameMatchs(ResultMetalModel metalModel);

        DataTable GetRacingMatch(ResultMetalModel metalModel);

        #endregion Report

        #region Popup

        DataTable GetPopUpMatch(int matchId, int raceNo, int licenseeId, int userId);

        DataTable GetPopUpParalayMatch(long refNo);

        DataTable GetPopUpNumberGameMatch(int matchId);

        #endregion Popup
    }
}