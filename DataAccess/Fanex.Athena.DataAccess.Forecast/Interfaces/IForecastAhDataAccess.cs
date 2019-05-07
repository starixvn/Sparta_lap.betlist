// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Data;
    using System.Threading.Tasks;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;

    public interface IForecastAhDataAccess
    {
        Task<ForecastAH> GetForecastTopOddsByMatch(int matchId, int betType);

        Task<DataTable> GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        Task<DataTable> GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);
    }
}