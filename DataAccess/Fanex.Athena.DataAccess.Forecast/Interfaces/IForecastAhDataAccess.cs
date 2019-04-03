// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Data;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;

    public interface IForecastAhDataAccess
    {
        ForecastAH GetForecastTopOddsByMatch(int matchId, int betType);

        DataTable GetForecastHandicap(ForecastAHMetaModel metaModel, int userId);

        DataTable GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId);
    }
}