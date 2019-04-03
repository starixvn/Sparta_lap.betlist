// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast
{
    using System.Collections.Generic;
    using DataAccess.Forecast.Interfaces;
    using Interfaces;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;

    public class ForecastOneXTwoService : IForecastOneXTwoService
    {
        private readonly IForecastOneXTwoDataAccess _forecastOneXTwoDataAccess;

        public ForecastOneXTwoService(IForecastOneXTwoDataAccess forecastOneXTwoDataAccess)
        {
            _forecastOneXTwoDataAccess = forecastOneXTwoDataAccess;
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastOneXTwoDataAccess.GetForecastOneXTwoByMatch(metaModel);
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastOneXTwoDataAccess.GetForecastOneXTwoByLeague(metaModel);
        }
    }
}