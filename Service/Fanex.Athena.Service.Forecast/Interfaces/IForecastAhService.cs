// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast.Interfaces
{
    using System.Threading.Tasks;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;

    public interface IForecastAhService
    {
        Task<ForecastAH> GetForecastTopOddsByMatch(int matchId, int betType);

        Task<ForecastValue> GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        Task<ForecastValue> GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);
    }
}