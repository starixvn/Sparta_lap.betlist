// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;

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

        IList<Racing> GetVirtualSportMatches(ResultMetalModel metalModel);

        #endregion Report

        #region Popup

        IList<T> GetPopUpMatches<T>(ResultPopupMetalModel metaModel);

        IList<Parlay> GetPopUpParalayMatch(long refNo);

        Sportsbook GetSportsbookGameDetail(int matchId);

        #endregion Popup
    }
}