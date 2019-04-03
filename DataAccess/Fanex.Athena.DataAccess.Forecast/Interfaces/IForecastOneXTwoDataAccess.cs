// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;

    public interface IForecastOneXTwoDataAccess
    {
        Task<IList<ForecastOneXTwo>> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel);

        Task<IList<ForecastOneXTwo>> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel);
    }
}