// --------------------------------------------------------------------------
// <copyright file="IForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast.Interfaces
{
    using System.Collections.Generic;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;

    public interface IForecastOneXTwoService
    {
        IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel);

        IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);
    }
}