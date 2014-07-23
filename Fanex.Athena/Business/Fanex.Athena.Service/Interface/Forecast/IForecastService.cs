// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Service.Interface.Forecast
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;
    using Models.DTO.Forecast.Forecast1X2;
    using Models.DTO.Forecast.ForecastAH;
    using Models.DTO.Forecast.ForecastHdou;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast interface.
    /// </summary>
    public interface IForecastService
    {
        #region Forecast AH

        ForecastAH GetFcTopOddsByMatch(int matchId, int betType);

        ForecastValue GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        ForecastValue GetForacastOverUnder(ForecastAHMetaModel metaModel, int userId);

        #endregion Forecast AH

        #region Forecast 1 x 2

        IList<League> GetFcRunningLeague(ForecastBaseMetaModel metaModel);

        IList<ForecastOneXTwo> GetFcOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId);

        IList<ForecastOneXTwo> GetFcOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);

        #endregion Forecast 1 x 2

        #region Forecast Handicap Over Under

        /// <summary>
        /// Get Forecast data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The data list.</returns>
        IList<ForecastHandicapOverUnder> GetFcHandicapOverUnder(ForecastHdpOuMetaModel metaModel);

        #endregion Forecast Handicap Over Under

        #region New Forecast

        /// <summary>
        /// Get forecast data.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="headerInfo">The data for header section.</param>
        /// <returns>The forecast data.</returns>
        NewForecastHdpOuData GetNewForecastData(NewForecastHdpOuMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo);

        #endregion New Forecast
    }
}