// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Service.Forecast.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    /// <summary>
    /// Forecast interface.
    /// </summary>
    public interface IForecastService
    {
        #region Forecast AH

        ForecastAH GetForecastTopOddsByMatch(int matchId, int betType);

        ForecastValue GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        ForecastValue GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);

        #endregion Forecast AH

        #region Forecast 1 x 2

        IList<League> GetForecastRunningLeague(ForecastBaseMetaModel metaModel);

        IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId);

        IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);

        #endregion Forecast 1 x 2

        #region Forecast Handicap Over Under

        /// <summary>
        /// Get Forecast data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The data list.</returns>
        IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel);

        #endregion Forecast Handicap Over Under

        #region New Forecast

        /// <summary>
        /// Get forecast data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="headerInfo">The data for header section.</param>
        /// <returns>The forecast data.</returns>
        NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo);

        #endregion New Forecast
    }
}