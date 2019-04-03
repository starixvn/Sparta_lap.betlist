// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast.Interfaces
{
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;

    public interface IForecastAhService
    {
        ForecastAH GetForecastTopOddsByMatch(int matchId, int betType);

        ForecastValue GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        ForecastValue GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);
    }
}