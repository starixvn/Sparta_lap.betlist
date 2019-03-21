// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;

    [System.CLSCompliant(false)]
    public interface IResultDataAccess
    {
        IList<Outright> GetOutRight(ResultMetaModel metal);

        #region Filter

        Task<IList<Sport>> GetSports(DateTime dateValue, int licenseeId);

        Task<IList<League>> GetLeagues(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        Task<IList<T>> GetMatches<T>(ResultMetaModel metalModel);

        IList<Racing> GetVirtualSportMatches(ResultMetaModel metalModel);

        #endregion Report

        #region Popup

        IList<T> GetPopUpMatches<T>(ResultPopupMetaModel metaModel);

        IList<Parlay> GetPopUpParalayMatch(long refNo);

        Sportsbook GetSportsbookGameDetail(int matchId);

        #endregion Popup
    }
}