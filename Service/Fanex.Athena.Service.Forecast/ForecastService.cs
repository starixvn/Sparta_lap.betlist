// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast
{
    using System.Collections.Generic;
    using DataAccess.Forecast.Interfaces;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Interfaces;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public class ForecastService : IForecastService
    {
        private readonly IForecastDataAccess _forecastDataAccess;

        public ForecastService(IForecastDataAccess forecastDataAccess)
        {
            _forecastDataAccess = forecastDataAccess;
        }

        #region Forecast Handicap Over/Under

        public IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastHandicapOverUnder(metaModel);
        }

        #endregion Forecast Handicap Over/Under

        #region New Forecast

        public NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo)
        {
            return metaModel.IsValid
                ? _forecastDataAccess.GetNewForecastData(metaModel, headerInfo)
                : new NewForecastHdpOuData();
        }

        #endregion New Forecast
    }
}