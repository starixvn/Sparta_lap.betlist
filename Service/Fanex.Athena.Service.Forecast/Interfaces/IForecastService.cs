// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public interface IForecastService
    {
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